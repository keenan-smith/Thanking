using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thanking.Attributes;
using Thanking.Components.Basic;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
    public class PlayerFinderData
    {
        public uint Ip { get; set; }
        public ushort Port { get; set; }
        public string PlayerName { get; set; }
        public string ServerName { get; set; }
        public gameserveritem_t RawDetails { get; set; }
    }

    public class PlayersQuery
    {
        private readonly uint ip;
        private readonly ushort port;
        private readonly ISteamMatchmakingPlayersResponse playersResponse;
        private readonly List<string> players = new List<string>();
        private readonly ManualResetEvent reset = new ManualResetEvent(false);
        private readonly CancellationToken cancellation;
        private HServerQuery query;

        public PlayersQuery(uint ip, ushort port, CancellationToken cancellation)
        {
            this.ip = ip;
            this.port = port;
            this.playersResponse = new ISteamMatchmakingPlayersResponse(
                new ISteamMatchmakingPlayersResponse.AddPlayerToList(OnAddPlayerToList),
                new ISteamMatchmakingPlayersResponse.PlayersFailedToRespond(OnPlayersFailedToRespond),
                new ISteamMatchmakingPlayersResponse.PlayersRefreshComplete(OnPlayersRefreshComplete));
        }

        public List<string> GetPlayers()
        {
            if (cancellation.IsCancellationRequested)
                return players;

            query = SteamMatchmakingServers.PlayerDetails(ip, (ushort)(port + 1), playersResponse);
            WaitHandle.WaitAny(new WaitHandle[] { reset, cancellation.WaitHandle });
            StopRequest();
            return players;
        }

        private void StopRequest()
        {
            if (query == HServerQuery.Invalid)
                return;
            SteamMatchmakingServers.CancelServerQuery(query);
            query = HServerQuery.Invalid;
        }

        private void OnAddPlayerToList(string name, int score, float time)
        {
            players.Add(name);
        }

        private void OnPlayersFailedToRespond()
        {
            reset.Set();
        }

        private void OnPlayersRefreshComplete()
        {
            reset.Set();
        }
    }

    public static class PlayerFinderTab
    {
        private static Vector2 foundScroll;

        // prop so that filter version is always up to date
        private static List<MatchMakingKeyValuePair_t> Filters
        {
            get => new List<MatchMakingKeyValuePair_t>()
                {
                    new MatchMakingKeyValuePair_t
                    {
                        m_szKey = "gamedir",
                        m_szValue = "unturned"
                    },
                    new MatchMakingKeyValuePair_t
                    {
                        m_szKey = "hasplayers",
                        m_szValue = "1"
                    },
                    new MatchMakingKeyValuePair_t
                    {
                        m_szKey = "gamedataand",
                        m_szValue = "," + Provider.APP_VERSION
                    }
                };
        }

        private static bool IsQueryingPlayers
        {
            get
            {
                lock (isQueryingPlayersLocker)
                    return isQueryingPlayers;
            }
            set
            {
                lock (isQueryingPlayersLocker)
                    isQueryingPlayers = value;
            }
        }

        private static bool isQueryingServerList;
        private static bool isQueryingPlayers;
        private static PlayerFinderData selectedDetail;
        private static int playersQueryCompleted;

        private static bool caseSensitive;
        private static bool exactMatch;
        private static string playerName;

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static readonly List<PlayerFinderData> playerHits = new List<PlayerFinderData>();
        private static readonly List<gameserveritem_t> gameservers = new List<gameserveritem_t>();
        private static ISteamMatchmakingServerListResponse serverListResponse;
        private static HServerListRequest serverRequest;
        private static float lastCancelledPlayerQuery;
        private static Thread playerQueryThread;

        // locking these two because modified in QueryPlayers thread
        private static readonly object playerHitsLocker = new object();
        private static readonly object isQueryingPlayersLocker = new object();

        [Initializer]
        public static void Initialize()
        {
            serverListResponse = new ISteamMatchmakingServerListResponse(
            new ISteamMatchmakingServerListResponse.ServerResponded(OnServerResponded),
            new ISteamMatchmakingServerListResponse.ServerFailedToRespond(OnServerFailedToRespond),
            new ISteamMatchmakingServerListResponse.RefreshComplete(OnRefreshComplete));
        }

        private static void OnRefreshComplete(HServerListRequest request, EMatchMakingServerResponse response)
        {
            isQueryingServerList = false;
        }

        private static void OnServerResponded(HServerListRequest request, int index)
        {
            gameservers.Add(SteamMatchmakingServers.GetServerDetails(request, index));
        }

        private static void OnServerFailedToRespond(HServerListRequest request, int index)
        {
        }
        
        private static void StopServerRequest()
        {
            if (serverRequest == HServerListRequest.Invalid)
                return;
            isQueryingServerList = false;
            SteamMatchmakingServers.ReleaseRequest(serverRequest);
            serverRequest = HServerListRequest.Invalid;
        }

        private static void ReloadServers()
        {
            playersQueryCompleted = 0;
            selectedDetail = null;

            lock (playerHitsLocker)
                playerHits.Clear();

            StopServerRequest();
            gameservers.Clear();
            isQueryingServerList = true;
            serverRequest = SteamMatchmakingServers.RequestInternetServerList(Provider.APP_ID, Filters.ToArray(), (uint)Filters.Count, serverListResponse);
        }

        private static void BeginQueryPlayers()
        {
            IsQueryingPlayers = true;
            playersQueryCompleted = 0;
            selectedDetail = null;

            lock (playerHitsLocker)
                playerHits.Clear();

            tokenSource = new CancellationTokenSource();
            playerQueryThread = new Thread(QueryPlayers);
            playerQueryThread.Start();
        }

        private static void QueryPlayers()
        {
            var loopOptions = new ParallelOptions { CancellationToken = tokenSource.Token };
            try
            {
                Parallel.ForEach(gameservers, loopOptions, (gameserver) =>
                {
                    var ip = gameserver.m_NetAdr.GetIP();
                    var port = gameserver.m_NetAdr.GetConnectionPort();
                    var players = new PlayersQuery(ip, port, loopOptions.CancellationToken).GetPlayers();

                    loopOptions.CancellationToken.ThrowIfCancellationRequested();

                    var comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                    var foundName = exactMatch ?
                        players.FirstOrDefault(x => x.Equals(playerName, comparisonType)) :
                        players.FirstOrDefault(x => x.IndexOf(playerName, comparisonType) >= 0);

                    if (foundName != null)
                    {
                        lock (playerHitsLocker)
                        {
                            playerHits.Add(new PlayerFinderData
                            {
                                Ip = ip,
                                Port = port,
                                PlayerName = foundName,
                                ServerName = gameserver.GetServerName(),
                                RawDetails = gameserver
                            });
                        }
                    }

                    Interlocked.Increment(ref playersQueryCompleted);
                });
            }
            catch (OperationCanceledException) { }
            finally
            {
                tokenSource.Dispose();
                IsQueryingPlayers = false;
            }
        }

        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 10, 466, 250 - 30), "Servers Found", ref foundScroll, () =>
            {
                lock (playerHitsLocker)
                {
                    foreach (var detail in playerHits)
                    {
                        if (Prefab.Button($"<b>{detail.PlayerName}</b> on {Parser.getIPFromUInt32(detail.Ip)}:{detail.Port}", 400))
                            selectedDetail = detail;
                        GUILayout.Space(2);
                    }
                }
            });

            Prefab.MenuArea(new Rect(0, 235, 190, 195), "Servers", () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label($"Servers Loaded: {gameservers.Count}");
                GUILayout.Space(1);

                if (!isQueryingServerList && !IsQueryingPlayers)
                {
                    if (Prefab.Button("Load Servers", 135))
                        ReloadServers();
                }
                else if (isQueryingServerList)
                {
                    GUILayout.Label("Querying for Servers...");
                    GUILayout.Space(2);

                    if (Prefab.Button("Cancel", 135))
                        StopServerRequest();
                }

                if (gameservers.Count > 0 && !isQueryingServerList)
                {
                    GUILayout.Space(1);
                    GUILayout.Label($"Servers Searched: {playersQueryCompleted}/{gameservers.Count}");
                    if (!IsQueryingPlayers)
                    {
                        GUILayout.Space(1);
                        playerName = Prefab.TextField(playerName, "Name: ", 95);
                        GUILayout.Space(1);
                        Prefab.Toggle("Case Sensitive", ref caseSensitive);
                        GUILayout.Space(1);
                        Prefab.Toggle("Exact Match", ref exactMatch);
                        GUILayout.Space(2);
                        if (Prefab.Button("Find Players", 135) && playerName.Trim() != String.Empty)
                            BeginQueryPlayers();
                    }
                    else
                    {
                        GUILayout.Label($"Searching for <b>{playerName}</b>");
                        GUILayout.Space(2);

                        if (!tokenSource.IsCancellationRequested)
                        {
                            if (Prefab.Button("Cancel", 135))
                            {
                                tokenSource.Cancel();
                                lastCancelledPlayerQuery = Time.realtimeSinceStartup;
                            }
                        }
                        else
                        {
                            var delta = Time.realtimeSinceStartup - lastCancelledPlayerQuery;
                            var text = $"Cancelling... ({(delta <= 5 ? (5 - (int)(delta)).ToString() : "Force")})";
                            // Screw it, I can't fix it
                            // Not really orthodox and probably doesn't clean up but who cares
                            if (Prefab.Button(text, 135) && delta > 5)
                            {
                                playerQueryThread.Abort();
                                tokenSource.Dispose();
                                IsQueryingPlayers = false;
                            }
                        }
                    }
                }


                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });

            Prefab.MenuArea(new Rect(190 + 6, 235, 270, 195), "Server", () =>
            {
                if (selectedDetail == null)
                    return;

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label("IP: ");
                GUILayout.TextField($"{Parser.getIPFromUInt32(selectedDetail.Ip)}:{selectedDetail.Port}", Prefab._TextStyle);
                GUILayout.Space(1);
                GUILayout.Label($"Player Name:\n{selectedDetail.PlayerName}");
                GUILayout.Space(1);
                GUILayout.Label($"Server Name:\n{selectedDetail.ServerName}");
                GUILayout.Space(2);

                if (!Provider.isConnected)
                {
                    if (Prefab.Button("Connect", 100))
                    {
                        MenuUI.closeAll();
                        MenuUI.closeAlert();
                        MenuPlayServerInfoUI.open(new SteamServerInfo(selectedDetail.RawDetails),
                            String.Empty, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
                    }
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            });
        }
    }
}
