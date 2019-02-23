using System;
using SDG.Unturned;
using Thinking.Attributes;
using Thinking.Components.Basic;
using Thinking.Coroutines;
using Thinking.Options;
using Thinking.Options.VisualOptions;
using Thinking.Utilities;
using Thinking.Variables;
using UnityEngine;
using HighlightingSystem;
using System.Collections.Generic;
using System.Linq;
using Thinking.Managers.Main;
using UnityEngine.PostProcessing;
using Thinking.Options.AimOptions;

namespace Thinking.Components.UI
{
	[SpyComponent]
	[Component]
	public class ESPComponent : MonoBehaviour
	{
		public static Material GLMat;
		public static Font ESPFont;

        public static List<Highlighter> Highlighters = new List<Highlighter>();

        public static Camera MainCamera;

		[Initializer]
		public static void OnInit()
		{
			for (int i = 0; i < ESPOptions.VisualOptions.Length; i++)
			{
				ColorUtilities.addColor(new Options.UIVariables.ColorVariable($"_{(ESPTarget)i}", $"ESP - {(ESPTarget)i}", Color.red, false));
				ColorUtilities.addColor(new Options.UIVariables.ColorVariable($"_{(ESPTarget)i}_Text", $"ESP - {(ESPTarget)i} (Text)", Color.white, false));
				ColorUtilities.addColor(new Options.UIVariables.ColorVariable($"_{(ESPTarget)i}_Outline", $"ESP - {(ESPTarget)i} (Outline)", Color.black, false));
				ColorUtilities.addColor(new Options.UIVariables.ColorVariable($"_{(ESPTarget)i}_Glow", $"ESP - {(ESPTarget)i} (Glow)", Color.yellow, false));
			}

			ColorUtilities.addColor(new Options.UIVariables.ColorVariable("_ESPFriendly", "Friendly Players", Color.green, false));
			ColorUtilities.addColor(new Options.UIVariables.ColorVariable("_ChamsFriendVisible", "Chams - Visible Friend", Color.green, false));
			ColorUtilities.addColor(new Options.UIVariables.ColorVariable("_ChamsFriendInisible", "Chams - Invisible Friend", Color.blue, false));
			ColorUtilities.addColor(new Options.UIVariables.ColorVariable("_ChamsEnemyVisible", "Chams - Visible Enemy", new Color32(255, 165, 0, 255), false));
			ColorUtilities.addColor(new Options.UIVariables.ColorVariable("_ChamsEnemyInvisible", "Chams - Invisible Enemy", Color.red, false));
		}
		
		public void Start()
        {
            CoroutineComponent.ESPCoroutine = StartCoroutine(ESPCoroutines.UpdateObjectList());
            CoroutineComponent.ChamsCoroutine = StartCoroutine(ESPCoroutines.DoChams());
	        MainCamera = OptimizationVariables.MainCam;
        }
		
		public void Update()
        {
            if (MiscOptions.NoRain)
                LevelLighting.rainyness = ELightingRain.NONE;
            if (MiscOptions.NoSnow)
                LevelLighting.snowyness = ELightingSnow.NONE;
        }

		public void OnGUI()
		{
			if (Event.current.type != EventType.Repaint || !ESPOptions.Enabled)
				return;

			if (!DrawUtilities.ShouldRun())
				return;

			GUI.depth = 1;

	        if (MainCamera == null)
		        MainCamera = OptimizationVariables.MainCam;

            Vector3 localPos = OptimizationVariables.MainPlayer.transform.position;

			Vector3 aimPos = OptimizationVariables.MainPlayer.look.aim.position;
			Vector3 aimForward = OptimizationVariables.MainPlayer.look.aim.forward;

            for (int i = 0; i < ESPVariables.Objects.Count; i++)
			{
				ESPObject obj = ESPVariables.Objects[i];
				ESPVisual visual = ESPOptions.VisualOptions[(int)obj.Target];

				GameObject go = obj.GObject;

				if (!visual.Enabled)
				{
					Highlighter highlighter = go.GetComponent<Highlighter>();
					if (highlighter != null && highlighter != TrajectoryComponent.Highlighted)
						highlighter.ConstantOffImmediate();	
					
					continue;
				}
                if (obj.Target == ESPTarget.Items && ESPOptions.FilterItems)
                    if (!ItemUtilities.Whitelisted(((InteractableItem)obj.Object).asset, ItemOptions.ItemESPOptions))
                        continue;

				Color c = ColorUtilities.getColor($"_{obj.Target}");
				LabelLocation ll = visual.Location;

				if (go == null)
					continue;

				Vector3 position = go.transform.position;
				double dist = VectorUtilities.GetDistance(position, localPos);

				if (dist < 0.5 || (dist > visual.Distance && !visual.InfiniteDistance))
					continue;

				Vector3 cpos = MainCamera.WorldToScreenPoint(position);

				if (cpos.z <= 0)
					continue;

				string text = "";

				Vector3 scale = go.transform.localScale;
				Bounds b;
				switch (obj.Target)
				{
					case ESPTarget.Players:
					case ESPTarget.Zombies:
						b = new Bounds(new Vector3(position.x, position.y + 1, position.z),
							new Vector3(scale.x * 2, scale.y * 3, scale.z * 2));
						break;
					case ESPTarget.Vehicles:
						
						b = go.transform.Find("Model_0").GetComponent<MeshRenderer>().bounds;
						Transform child = go.transform.Find("Model_1");
						
						if (child != null)
							b.Encapsulate(child.GetComponent<MeshRenderer>().bounds);
						
						break;
					default:
						b = go.GetComponent<Collider>().bounds;
						break;
				}

				int size = DrawUtilities.GetTextSize(visual, dist);
				double rounded = Math.Round(dist);

				/*#if DEBUG
				DebugUtilities.Log(obj.Target.ToString()); //Holy fuck nuggets this is laggy
				#endif*/

				string outerText = $"<size={size}>";
				text = $"<size={size}>";
				
				switch (obj.Target)
				{
					#region Players

					case ESPTarget.Players:
					{
						Player p = (Player) obj.Object;

						if (p.life.isDead)
							continue;

						if (visual.ShowName)
							text += p.name + "\n";
                        if (RaycastUtilities.TargetedPlayer == p && RaycastOptions.EnablePlayerSelection)
                            text += "[Targeted]\n";
						if (ESPOptions.ShowPlayerWeapon)
							text += (p.equipment.asset != null ? p.equipment.asset.itemName : "Fists") + "\n";
						if (ESPOptions.ShowPlayerVehicle)
							text += (p.movement.getVehicle() != null ? p.movement.getVehicle().asset.name + "\n" : "No Vehicle\n");

						b.size = b.size / 2;
						b.size = new Vector3(b.size.x, b.size.y * 1.25f, b.size.z);

						if (FriendUtilities.IsFriendly(p) && ESPOptions.UsePlayerGroup)
							c = ColorUtilities.getColor("_ESPFriendly");

						break;
					}

					#endregion

					#region Zombies

					case ESPTarget.Zombies:
					{
						if (((Zombie) obj.Object).isDead)
							continue;
						
						if (visual.ShowName)
							text += $"Zombie\n";
						
						break;
					}

					#endregion

					#region Items

					case ESPTarget.Items:
					{
						InteractableItem item = (InteractableItem) obj.Object;

						if (visual.ShowName)
							text += item.asset.itemName + "\n";
						
						break;
					}

					#endregion

					#region Sentries

					case ESPTarget.Sentries:
					{
						InteractableSentry sentry = (InteractableSentry) obj.Object;

						if (visual.ShowName)
						{
							text += "Sentry\n";
							outerText += "Sentry\n";
						}

						if (ESPOptions.ShowSentryItem)
						{
							outerText += SentryName(sentry.displayItem, false) + "\n"; 
							text += SentryName(sentry.displayItem, true) + "\n";
						}
						
						break;
					}

					#endregion

					#region Beds

					case ESPTarget.Beds:
					{
						InteractableBed bed = (InteractableBed) obj.Object;

						if (visual.ShowName)
						{
							text += "Bed\n";
							outerText += "Bed\n";
						}

						if (ESPOptions.ShowClaimed)
						{
							text += GetOwned(bed, true) + "\n";
							outerText += GetOwned(bed, false) + "\n";
						}
						break;
					}

					#endregion

					#region Claim Flags

					case ESPTarget.ClaimFlags:
					{
						if (visual.ShowName)
							text += "Claim Flag\n";
						
						break;
					}

					#endregion

					#region Vehicles

					case ESPTarget.Vehicles:
					{
						InteractableVehicle vehicle = (InteractableVehicle) obj.Object;
						
						if (vehicle.health == 0)
							continue;

						if (ESPOptions.FilterVehicleLocked && vehicle.isLocked)
							continue;

						vehicle.getDisplayFuel(out ushort Fuel, out ushort MaxFuel);
						
						float health = Mathf.Round(100 * (vehicle.health / (float) vehicle.asset.health));
						float fuel =  Mathf.Round(100 * (Fuel / (float) MaxFuel));

						if (visual.ShowName)
						{
							text += vehicle.asset.name + "\n";	
							outerText += vehicle.asset.name + "\n";	
						}

						if (ESPOptions.ShowVehicleHealth)
						{
							text += $"Health: {health}%\n";
							outerText += $"Health: {health}%\n";
						}

						if (ESPOptions.ShowVehicleFuel)
						{
							text += $"Fuel: {fuel}%\n";
							outerText += $"Fuel: {fuel}%\n";
						}

						if (ESPOptions.ShowVehicleLocked)
						{
							text += GetLocked(vehicle, true) + "\n";
							outerText += GetLocked(vehicle, false) + "\n";
						}
						
						break;
					}

					#endregion

					#region Storage

					case ESPTarget.Storage:
					{
						if (visual.ShowName)
							text += "Storage\n";
						
						break;
					}

					#endregion

					#region Generators

					case ESPTarget.Generators:
					{
						InteractableGenerator gen = (InteractableGenerator) obj.Object;

						float fuel =  Mathf.Round(100 * (gen.fuel / (float) gen.capacity));

						if (ESPOptions.ShowGeneratorFuel)
						{
							text += $"Fuel: {fuel}%\n";
							outerText += $"Fuel: {fuel}%\n";
						}

						if (ESPOptions.ShowGeneratorPowered)
						{
							text += GetPowered(gen, true) + "\n";
							outerText += GetPowered(gen, false) + "\n";
						}
						
						break;
					}

					#endregion
				}

				if (outerText == $"<size={size}>")
					outerText = null;
				
				if (visual.ShowDistance)
				{
					text += $"{rounded}m\n";
					
					if (outerText != null)
						outerText += $"{rounded}m\n";
				}

				if (visual.ShowAngle)
				{
					double roundedFOV = Math.Round(VectorUtilities.GetAngleDelta(aimPos, aimForward, position), 2);
					text += $"Angle: {roundedFOV}°\n";
					
					if (outerText != null)
						outerText += $"{roundedFOV}°\n";
				}

				text += "</size>";

				if (outerText != null)
					outerText += "</size>";
				
				Vector3[] vectors = DrawUtilities.GetBoxVectors(b);
				
				Vector2[] W2SVectors = DrawUtilities.GetRectangleLines(MainCamera, b, c);
				Vector3 LabelVector = DrawUtilities.Get2DW2SVector(MainCamera, W2SVectors, ll);

				if (MirrorCameraOptions.Enabled && W2SVectors.Any(v => MirrorCameraComponent.viewport.Contains(v)))
				{
					Highlighter highlighter = go.GetComponent<Highlighter>();
					if (highlighter != null)
						highlighter.ConstantOffImmediate();
					
					continue;
				}
				
                if (visual.Boxes)
                {
	                if (visual.TwoDimensional)
		                DrawUtilities.PrepareRectangleLines(W2SVectors, c);
	                else
	                {
		                DrawUtilities.PrepareBoxLines(vectors, c);
		                LabelVector = DrawUtilities.Get3DW2SVector(MainCamera, b, ll);
	                }
                }

                if (visual.Glow)
                {
                    Highlighter highlighter = go.GetComponent<Highlighter>() ?? go.AddComponent<Highlighter>();
                    highlighter.OccluderOn();
                    highlighter.SeeThroughOn();
                    highlighter.ConstantOnImmediate(ColorUtilities.getColor($"_{obj.Target}_Glow"));
                    Highlighters.Add(highlighter);
                }
                else
                {
                    Highlighter highlighter = go.GetComponent<Highlighter>();
                    if (highlighter != null && highlighter != TrajectoryComponent.Highlighted)
	                    highlighter.ConstantOffImmediate();
                }

                if (visual.Labels)
	                DrawUtilities.DrawLabel(ESPFont, ll, LabelVector, text, ColorUtilities.getColor($"_{obj.Target}_Text"), ColorUtilities.getColor($"_{obj.Target}_Outline"), visual.BorderStrength, outerText);

				if (visual.LineToObject)
					ESPVariables.DrawBuffer2.Enqueue(new ESPBox2
					{
						Color = c,
						Vertices = new[]
						{
							new Vector2(Screen.width / 2, Screen.height),
							new Vector2(cpos.x, Screen.height - cpos.y)
						}
					});
			}
			
			GLMat.SetPass(0);

			GL.PushMatrix();
			GL.LoadProjectionMatrix(MainCamera.projectionMatrix);
			GL.modelview = MainCamera.worldToCameraMatrix;
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer.Count; i++)
			{
				ESPBox box = ESPVariables.DrawBuffer.Dequeue();

				GL.Color(box.Color);

				Vector3[] vertices = box.Vertices;
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex(vertices[j]);

			}
			GL.End();
			GL.PopMatrix();

			GL.PushMatrix();
			GL.Begin(GL.LINES);

			for (int i = 0; i < ESPVariables.DrawBuffer2.Count; i++)
			{
				ESPBox2 box = ESPVariables.DrawBuffer2.Dequeue();

				GL.Color(box.Color);
				Vector2[] vertices = box.Vertices;

				bool Run = true;
				
				for (int j = 0; j < vertices.Length; j++)
					if (j < vertices.Length - 1)
					{
						Vector2 v1 = vertices[j];
						Vector2 v2 = vertices[j + 1];
						
						if (Vector2.Distance(v2, v1) > Screen.width / 2)
						{
							Run = false;
							break;
						}
					}

				if (!Run)
					continue;
				
				for (int j = 0; j < vertices.Length; j++)
					GL.Vertex3(vertices[j].x, vertices[j].y, 0);

			}
			GL.End();
			GL.PopMatrix();
        }

        [OnSpy]
        public static void DisableHighlighters()
        {
            foreach(Highlighter highlighter in Highlighters) // pls dont go apeshit kr4ken its only called once every spy and it's not like update() where its called every milisecond
            {
                highlighter.ConstantOffImmediate();
            }
            Highlighters.Clear();
        }

		public static string SentryName(Item DisplayItem, bool color) => DisplayItem != null
			? Assets.find(EAssetType.ITEM, DisplayItem.id).name
			: color ? "<color=#ff0000ff>No Item</color>" : "No Item";

		public static string GetLocked(InteractableVehicle Vehicle, bool color) =>
			Vehicle.isLocked ? color ? "<color=#ff0000ff>LOCKED</color>" : "LOCKED" :
			color ? "<color=#00ff00ff>UNLOCKED</color>" : "UNLOCKED";

		public static string GetPowered(InteractableGenerator Generator, bool color) =>
			Generator.isPowered ? color ? "<color=#00ff00ff>ON</color>" : "ON" :
			color ? "<color=#ff0000ff>OFF</color>" : "OFF";

		public static string GetOwned(InteractableBed bed, bool color) =>
			bed.isClaimed ? color ? "<color=$00ff00ff>CLAIMED</color>" : "CLAIMED" :
			color ? "<color=#ff0000ff>UNCLAIMED</color>" : "UNCLAIMED";
	}
}
