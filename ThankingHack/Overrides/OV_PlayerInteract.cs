using SDG.Unturned;
using System.Reflection;
using Thanking.Attributes;
using UnityEngine;
using SDG.Framework.Utilities;
using Thanking.Utilities;
using static Thanking.Utilities.ReflectionUtilities;

namespace Thanking.Overrides
{
	public class OV_PlayerInteract : PlayerCaller
	{
		public static Interactable interactable
		{
			get
			{
				return _interactable;
			}
		}

		// SDG.Unturned.PlayerInteract
		public static Interactable2 interactable2
		{
			get
			{
				return _interactable2;
			}
		}

		// SDG.Unturned.PlayerInteract
		private float salvageTime
		{
			get
			{
				if (Provider.isServer || base.channel.owner.isAdmin)
				{
					return 1f;
				}
				return 8f;
			}
		}

		private static Transform focus;
		private static RaycastHit hit;
		private static bool isHoldingKey;
		private static float lastInteract;
		private static float lastKeyDown;
		private static ItemAsset purchaseAsset => Player.player.interact.GetField<ItemAsset>("purchaseAsset", FieldType.PrivateStatic);
		private static Transform target;
		private static Interactable _interactable;
		private static Interactable2 _interactable2;

		[Override(typeof(PlayerInteract), "Update", BindingFlags.NonPublic | BindingFlags.Instance)]
		private void Update()
		{
			if (player.stance.stance == EPlayerStance.DRIVING || player.stance.stance == EPlayerStance.SITTING || player.life.isDead || player.workzone.isBuilding)
				return;

			if (Time.realtimeSinceStartup - lastInteract > 0.01f)
			{
				lastInteract = Time.realtimeSinceStartup;
				if (player.look.isCam)
				{
					PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out hit, 20f, (RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.ITEM | RayMasks.RESOURCE), QueryTriggerInteraction.UseGlobal);
					//PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out hit, 4f, RayMasks.PLAYER_INTERACT, QueryTriggerInteraction.UseGlobal);
				}
				else
				{
					PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out hit, (float)((player.look.perspective != EPlayerPerspective.THIRD) ? 20 : 24), (RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.ITEM | RayMasks.RESOURCE), QueryTriggerInteraction.UseGlobal);
					//PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out hit, (float)((player.look.perspective != EPlayerPerspective.THIRD) ? 4 : 6), RayMasks.PLAYER_INTERACT, QueryTriggerInteraction.UseGlobal);
				}
			}
			if (hit.transform != focus)
			{
				if (focus != null && interactable != null)
				{
					InteractableDoorHinge component = focus.GetComponent<InteractableDoorHinge>();
					if (component != null)
						HighlighterTool.unhighlight(focus.parent.parent);
					else
						HighlighterTool.unhighlight(focus);
				}
				focus = null;
				target = null;
				_interactable = null;
				_interactable2 = null;
				if (hit.transform != null)
				{
					focus = hit.transform;
					_interactable = focus.GetComponent<Interactable>();
					_interactable2 = focus.GetComponent<Interactable2>();
					if (interactable != null)
					{
						target = focus.FindChildRecursive("Target");
						if (interactable.checkInteractable())
						{
							if (PlayerUI.window.isEnabled)
							{
								if (interactable.checkUseable())
								{
									Color color;
									if (!interactable.checkHighlight(out color))
									{
										color = Color.green;
									}
									InteractableDoorHinge component2 = focus.GetComponent<InteractableDoorHinge>();
									if (component2 != null)
									{
										HighlighterTool.highlight(focus.parent.parent, color);
									}
									else
									{
										HighlighterTool.highlight(focus, color);
									}
								}
								else
								{
									Color color = Color.red;
									InteractableDoorHinge component3 = focus.GetComponent<InteractableDoorHinge>();
									if (component3 != null)
										HighlighterTool.highlight(focus.parent.parent, color);
									else
										HighlighterTool.highlight(focus, color);
								}
							}
						}
						else
						{
							target = null;
							_interactable = null;
						}
					}
				}
			}
			else
			{
				if (focus != null && interactable != null)
				{
					InteractableDoorHinge component4 = focus.GetComponent<InteractableDoorHinge>();
					if (component4 != null)
					{
						HighlighterTool.unhighlight(focus.parent.parent);
					}
					else
					{
						HighlighterTool.unhighlight(focus);
					}
				}
				focus = null;
				target = null;
				_interactable = null;
				_interactable2 = null;
			}

			if (player.life.isDead)
				return;

			if (interactable != null)
			{
				EPlayerMessage message;
				string text;
				Color color2;
				if (interactable.checkHint(out message, out text, out color2) && !PlayerUI.window.showCursor)
				{
					if (interactable.CompareTag("Item"))
					{
						PlayerUI.hint((!(target != null)) ? focus : target, message, text, color2, new object[]
						{
								((InteractableItem) interactable).item,
								((InteractableItem) interactable).asset
});
					}
					else
					{
						PlayerUI.hint((!(target != null)) ? focus : target, message, text, color2, new object[0]);
					}
				}
			}
			else if (purchaseAsset != null && player.movement.purchaseNode != null && !PlayerUI.window.showCursor)
			{
				PlayerUI.hint(null, EPlayerMessage.PURCHASE, string.Empty, Color.white, new object[]
				{
						purchaseAsset.itemName,
						player.movement.purchaseNode.cost
				});
			}
			else if (focus != null && focus.CompareTag("Enemy"))
			{
				Player player = DamageTool.getPlayer(focus);
				if (player != null && player != Player.player && !PlayerUI.window.showCursor)
				{
					PlayerUI.hint(null, EPlayerMessage.ENEMY, string.Empty, Color.white, new object[]
					{
							player.channel.owner
					});
				}
			}
			EPlayerMessage message2;
			float data;
			if (interactable2 != null && interactable2.checkHint(out message2, out data) && !PlayerUI.window.showCursor)
			{
				PlayerUI.hint2(message2, (!isHoldingKey) ? 0f : ((Time.realtimeSinceStartup - lastKeyDown) / this.salvageTime), data);
			}
			if ((player.stance.stance == EPlayerStance.DRIVING || player.stance.stance == EPlayerStance.SITTING) && !Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKeyDown(KeyCode.F1))
					VehicleManager.swapVehicle(0);
				if (Input.GetKeyDown(KeyCode.F2))
					VehicleManager.swapVehicle(1);
				if (Input.GetKeyDown(KeyCode.F3))
					VehicleManager.swapVehicle(2);
				if (Input.GetKeyDown(KeyCode.F4))
					VehicleManager.swapVehicle(3);
				if (Input.GetKeyDown(KeyCode.F5))
					VehicleManager.swapVehicle(4);
				if (Input.GetKeyDown(KeyCode.F6))
					VehicleManager.swapVehicle(5);
				if (Input.GetKeyDown(KeyCode.F7))
					VehicleManager.swapVehicle(6);
				if (Input.GetKeyDown(KeyCode.F8))
					VehicleManager.swapVehicle(7);
				if (Input.GetKeyDown(KeyCode.F9))
					VehicleManager.swapVehicle(8);
				if (Input.GetKeyDown(KeyCode.F10))
					VehicleManager.swapVehicle(9);
			}
			if (Input.GetKeyDown(ControlsSettings.interact))
			{
				lastKeyDown = Time.realtimeSinceStartup;
				isHoldingKey = true;
			}
			if (Input.GetKeyDown(ControlsSettings.inspect) && ControlsSettings.inspect != ControlsSettings.interact && player.equipment.canInspect)
			{
				channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
			if (isHoldingKey)
			{
				if (Input.GetKeyUp(ControlsSettings.interact))
				{
					isHoldingKey = false;
					if (PlayerUI.window.showCursor)
					{
						if (player.inventory.isStoring)
						{
							PlayerDashboardUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeSignUI.active)
						{
							PlayerBarricadeSignUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeStereoUI.active)
						{
							PlayerBarricadeStereoUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeLibraryUI.active)
						{
							PlayerBarricadeLibraryUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerBarricadeMannequinUI.active)
						{
							PlayerBarricadeMannequinUI.close();
							PlayerLifeUI.open();
						}
						else if (PlayerNPCDialogueUI.active)
						{
							if (PlayerNPCDialogueUI.dialogueAnimating)
							{
								PlayerNPCDialogueUI.skipText();
							}
							else if (PlayerNPCDialogueUI.dialogueHasNextPage)
							{
								PlayerNPCDialogueUI.nextPage();
							}
							else
							{
								PlayerNPCDialogueUI.close();
								PlayerLifeUI.open();
							}
						}
						else if (PlayerNPCQuestUI.active)
						{
							PlayerNPCQuestUI.closeNicely();
						}
						else if (PlayerNPCVendorUI.active)
						{
							PlayerNPCVendorUI.closeNicely();
						}
					}
					else if (player.stance.stance == EPlayerStance.DRIVING || player.stance.stance == EPlayerStance.SITTING)
					{
						VehicleManager.exitVehicle();
					}
					else if (focus != null && interactable != null)
					{
						if (interactable.checkUseable())
						{
							interactable.use();
						}
					}
					else if (purchaseAsset != null)
					{
						if (player.skills.experience >= player.movement.purchaseNode.cost)
						{
							player.skills.sendPurchase(player.movement.purchaseNode);
						}
					}
					else if (ControlsSettings.inspect == ControlsSettings.interact && player.equipment.canInspect)
					{
						channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
					}
				}
				else if (Time.realtimeSinceStartup - lastKeyDown > this.salvageTime)
				{
					isHoldingKey = false;
					if (!PlayerUI.window.showCursor && interactable2 != null)
					{
						interactable2.use();
					}
				}
			}
		}
	}
}