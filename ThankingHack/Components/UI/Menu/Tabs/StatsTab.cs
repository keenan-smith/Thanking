using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Options.UIVariables;
using UnityEngine;

namespace Thanking.Components.UI.Menu.Tabs
{
	public static class StatsTab
	{
		public static int Selected = 0;
		public static Vector2 ScrollPos;
		public static string Amount = "";

		public static string[] StatLabels =
		{
			"None",
			"Normal Zombie Kills",
			"Player Kills",
			"Items Found",
			"Resources Found",
			"Experience Found",
			"Mega Zombie Kills",
			"Player Deaths",
			"Animal Kills",
			"Blueprints Found",
			"Fishies Found",
			"Plants Taken",
			"Accuracy",
			"Headshots",
			"Foot Traveled",
			"Vehicle Traveled",
			"Arena Wins",
			"Buildables Taken",
			"Throwables Found"
		};

		public static string[] StatNames =
		{
			"None",
			"Kills_Zombies_Normal",
			"Kills_Players",
			"Found_Items",
			"Found_Resources",
			"Found_Experience",
			"Kills_Zombies_Mega",
			"Deaths_Players",
			"Kills_Animals",
			"Found_Crafts",
			"Found_Fishes",
			"Found_Plants",
			"Accuracy_Shot",
			"Headshots",
			"Travel_Foot",
			"Travel_Vehicle",
			"Arena_Wins",
			"Found_Buildables",
			"Found_Throwables"
		};

		public static void Tab()
		{
			Prefab.ScrollView(new Rect(0, 0, 250, 436), "STATS", ref ScrollPos, () =>
			{
				for (int i = 0; i < StatLabels.Length; i++)
				{
					string stat = StatLabels[i];
					if (Prefab.Button(stat, 205))
						Selected = i;

					GUILayout.Space(3);
				}
			});
			
			Rect setRect = new Rect(250 + 10, 0, 436 - 230 - 10, 250);
			Prefab.MenuArea(setRect, "Modify", () =>
			{
				if (Selected == 0)
					return;

				string Label = StatLabels[Selected];
				Provider.provider.statisticsService.userStatisticsService.getStatistic(StatNames[Selected], out int current);

				GUILayout.Label(Label, Prefab._TextStyle);
				GUILayout.Space(4);
				GUILayout.Label($"Current: {current}", Prefab._TextStyle);
				GUILayout.Space(3);
				Amount = Prefab.TextField(Amount, "Modifier: ", 50);
				GUILayout.Space(2);

				if (!int.TryParse(Amount, out int amt))
					return;

				if (Prefab.Button("Add", 75))
				{
					for (int i = 1; i <= amt; i++)
						Provider.provider.statisticsService.userStatisticsService.setStatistic(StatNames[Selected], current + i);
				}
			});
		}
	}
}
