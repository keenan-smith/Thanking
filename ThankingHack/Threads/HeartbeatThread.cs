using System.Diagnostics;
using System.Net;
using System.Threading;
using Thanking.Attributes;
using Thanking.Utilities;

namespace Thanking.Threads
{
	public static class HeartbeatThread
	{
		public static string hwid = null;
		#if Commercial
		//[Thread]
		public static void Start()
		{
			try
			{
				hwid = HWIDUtilities.GetHWID();
				while (true)
				{
					using (WebClient c = new WebClient())
					{
						c.Proxy = new WebProxy();
						string URI = "http://debug.ironic.services/api/download.php";
						string parameters = $"stage=4&steam_64={SDG.Unturned.Provider.client.m_SteamID}&steam_name={SDG.Unturned.Provider.clientName}&HWID={hwid}";
						c.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
						string result = c.UploadString(URI, parameters);

						if (!result.Contains("NWu5mU3bGK9bJMbvOB+mbC5S5Sz7ekahgTyqkeF0GBXBBUPCUtqwaZa4m65c9tTg"))
							Process.GetCurrentProcess().Kill();
					}

					Thread.Sleep(30000);
				}
			}
			catch
			{
				Process.GetCurrentProcess().Kill();
			}
		}
		#endif
	}
}
