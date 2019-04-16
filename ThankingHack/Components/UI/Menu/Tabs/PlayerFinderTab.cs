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

        public PlayersQuery(uint ip, ushort port)
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
            SteamMatchmakingServers.PlayerDetails(ip, (ushort)(port + 1), playersResponse);
            reset.WaitOne();
            return players;
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

        private static bool isQueryingServerList;
        private static bool isQueryingPlayers;
        private static PlayerFinderData selectedDetail;
        private static int playersQueryCompleted;

        private static bool caseSensitive;
        private static bool exactMatch;
        private static string playerName;

        private static readonly List<PlayerFinderData> playerHits = new List<PlayerFinderData>();
        private static readonly List<gameserveritem_t> gameservers = new List<gameserveritem_t>();
        private static ISteamMatchmakingServerListResponse serverListResponse;
        private static HServerListRequest serverRequest;

        private static readonly object locker = new object();

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
            playerHits.Clear();
            StopServerRequest();
            gameservers.Clear();
            isQueryingServerList = true;
            serverRequest = SteamMatchmakingServers.RequestInternetServerList(Provider.APP_ID, Filters.ToArray(), (uint)Filters.Count, serverListResponse);
        }

        private static void BeginQueryPlayers()
        {
            isQueryingPlayers = true;
            playersQueryCompleted = 0;
            selectedDetail = null;
            playerHits.Clear();
            new Thread(QueryPlayers).Start();
        }

        private static void QueryPlayers()
        {
            Parallel.ForEach(gameservers, (gameserver) =>
            {
                var ip = gameserver.m_NetAdr.GetIP();
                var port = gameserver.m_NetAdr.GetConnectionPort();
                var players = new PlayersQuery(ip, port).GetPlayers();
                var comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                var foundName = exactMatch ?
                    players.FirstOrDefault(x => x.Equals(playerName, comparisonType)) :
                    players.FirstOrDefault(x => x.IndexOf(playerName, comparisonType) >= 0);

                if (foundName != null)
                {
                    lock (locker)
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

            isQueryingPlayers = false;
        }

        public static void Tab()
        {
            Prefab.ScrollView(new Rect(0, 10, 466, 250 - 30), "Servers Found", ref foundScroll, () =>
            {
                foreach (var detail in playerHits)
                {
                    if (Prefab.Button($"<b>{detail.PlayerName}</b> on {Parser.getIPFromUInt32(detail.Ip)}:{detail.Port}", 400))
                        selectedDetail = detail;
                    GUILayout.Space(2);
                }
            });

            Prefab.MenuArea(new Rect(0, 235, 190, 195), "Servers", () =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();

                GUILayout.Label($"Servers Loaded: {gameservers.Count}");
                GUILayout.Space(1);

                if (!isQueryingServerList && !isQueryingPlayers)
                {
                    if (Prefab.Button("Load Servers", 135))
                        ReloadServers();
                }
                else if (isQueryingServerList)
                {
                    GUILayout.Label("Querying for Servers...");
                    GUILayout.Space(2);

                    if (Prefab.Button("Cancel", 135))
                    {
                        StopServerRequest();
                        gameservers.Clear();
                    }
                }

                if (gameservers.Count > 0 && !isQueryingServerList)
                {
                    GUILayout.Space(1);
                    GUILayout.Label($"Servers Searched: {playersQueryCompleted}/{gameservers.Count}");
                    if (!isQueryingPlayers)
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
