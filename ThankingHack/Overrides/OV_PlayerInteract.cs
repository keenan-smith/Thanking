using System.Reflection;
using HighlightingSystem;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Thanking.Attributes;
using Thanking.Coroutines;
using Thanking.Options;
using Thanking.Utilities;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Overrides
{
    public class OV_PlayerInteract
    {
        private static FieldInfo FocusField;
        private static FieldInfo TargetField;
        private static FieldInfo InteractableField;
        private static FieldInfo Interactable2Field;
        private static FieldInfo PurchaseAssetField;

        [Initializer]
        public static void Init()
        {
            FocusField = typeof(PlayerInteract).GetField("focus", ReflectionVariables.PrivateStatic);
            TargetField = typeof(PlayerInteract).GetField("target", ReflectionVariables.PrivateStatic);
            InteractableField = typeof(PlayerInteract).GetField("_interactable", ReflectionVariables.PrivateStatic);
            Interactable2Field = typeof(PlayerInteract).GetField("_interactable2", ReflectionVariables.PrivateStatic);
            PurchaseAssetField = typeof(PlayerInteract).GetField("purchaseAsset", ReflectionVariables.PrivateStatic);
        }

        #region Fields

        public static Transform focus
        {
            get =>
                (Transform)FocusField.GetValue(null);

            set =>
                FocusField.SetValue(null, value);
        }

        public static Transform target
        {
            get =>
                (Transform)TargetField.GetValue(null);

            set =>
                TargetField.SetValue(null, value);
        }

        public static Interactable interactable
        {
            get =>
                (Interactable)InteractableField.GetValue(null);

            set =>
                InteractableField.SetValue(null, value);
        }


        public static Interactable2 interactable2
        {
            get =>
                (Interactable2)Interactable2Field.GetValue(null);

            set =>
                Interactable2Field.SetValue(null, value);
        }

        public static ItemAsset purchaseAsset
        {
            get =>
                (ItemAsset)PurchaseAssetField.GetValue(null);

            set =>
                PurchaseAssetField.SetValue(null, value);
        }

        private float salvageTime =>
            MiscOptions.SalvageTime;

        private void hotkey(byte button) =>
            VehicleManager.swapVehicle(button);

        private static bool isHoldingKey;

        private static float lastInteract;

        private static float lastKeyDown;

        private static RaycastHit hit;

        private void onPurchaseUpdated(PurchaseNode node)
        {
            if (node == null)
                purchaseAsset = null;
            else
                purchaseAsset = (ItemAsset)Assets.find(EAssetType.ITEM, node.id);
        }

        #endregion

        #region ic3_is_gay_i_dont_know_what_to_name_this
        public static void highlight(Transform target, Color color)
        {
            if (target.CompareTag("Player") || target.CompareTag("Enemy") || target.CompareTag("Zombie") || target.CompareTag("Animal") || target.CompareTag("Agent"))
                return;
            Highlighter highlighter = target.GetComponent<Highlighter>();
            if (highlighter == null)
                highlighter = target.gameObject.AddComponent<Highlighter>();
            highlighter.ConstantOn(color);
            highlighter.SeeThroughOn();
        }
        #endregion

        #region Overriden Methods

        [Override(typeof(PlayerInteract), "Update", BindingFlags.NonPublic | BindingFlags.Instance)]
        public void OV_Update() // i have no idea what any of this does tbh
        {

            if (!DrawUtilities.ShouldRun() || PlayerCoroutines.IsSpying)
                return;

            if (Player.player.stance.stance != EPlayerStance.DRIVING && Player.player.stance.stance != EPlayerStance.SITTING &&
                !Player.player.life.isDead && !Player.player.workzone.isBuilding)
            {
                if (Time.realtimeSinceStartup - lastInteract > 0.1f)
                {
                    int Mask = 0;

                    if (InteractionOptions.HitBarricades)
                        Mask = Mask | RayMasks.BARRICADE;

                    if (InteractionOptions.HitItems)
                        Mask = Mask | RayMasks.ITEM;

                    if (InteractionOptions.HitResources)
                        Mask = Mask | RayMasks.RESOURCE;

                    if (InteractionOptions.HitStructures)
                        Mask = Mask | RayMasks.STRUCTURE;

                    if (InteractionOptions.HitVehicles)
                        Mask = Mask | RayMasks.VEHICLE;

                    lastInteract = Time.realtimeSinceStartup;

                    // ic3 has tamed this area with cancerous code
                    float Range = PlayerCoroutines.IsSpying ? 4 :
                        InteractionOptions.InteractThroughWalls ? 20f : 4f;

                    int RayMask = PlayerCoroutines.IsSpying ? RayMasks.PLAYER_INTERACT :
                        InteractionOptions.InteractThroughWalls ? Mask : RayMasks.PLAYER_INTERACT;

                    PlayerLook pLook = Player.player.look;

                    Vector3 origin = pLook.isCam ? pLook.aim.position : MainCamera.instance.transform.position;
                    Vector3 direction = pLook.isCam ? pLook.aim.forward : MainCamera.instance.transform.forward;

                    PhysicsUtility.raycast(new Ray(origin, direction), out hit, Player.player.look.perspective == EPlayerPerspective.THIRD ? Range + 2 : Range, RayMask, 0);

                    /*if (Player.player.look.isCam)
						PhysicsUtility.raycast(new Ray(Player.player.look.aim.position, Player.player.look.aim.forward),
							out hit, 20f, Mask);
					else
						PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward),
							out hit, Player.player.look.perspective != EPlayerPerspective.THIRD ? 20 : 24, Mask);*/
                }

                if (hit.transform != focus)
                {
                    if (focus != null && PlayerInteract.interactable != null)
                    {
                        InteractableDoorHinge component = focus.GetComponent<InteractableDoorHinge>();

                        HighlighterTool.unhighlight(component != null ? focus.parent.parent : focus);
                    }

                    focus = null;
                    target = null;
                    interactable = null;
                    interactable2 = null;

                    if (hit.transform != null)
                    {
                        focus = hit.transform;
                        interactable = focus.GetComponent<Interactable>();
                        interactable2 = focus.GetComponent<Interactable2>();

                        if (PlayerInteract.interactable != null)
                        {
                            target = focus.FindChildRecursive("Target");

                            if (PlayerInteract.interactable.checkInteractable())
                            {
                                if (PlayerUI.window.isEnabled)
                                {
                                    if (PlayerInteract.interactable.checkUseable())
                                    {
                                        if (!PlayerInteract.interactable.checkHighlight(out Color color))
                                            color = Color.green;

                                        InteractableDoorHinge component2 = focus.GetComponent<InteractableDoorHinge>();
                                        highlight(component2 != null ? focus.parent.parent : focus, color);
                                    }
                                    else
                                    {
                                        Color color = Color.red;
                                        InteractableDoorHinge component3 = focus.GetComponent<InteractableDoorHinge>();
                                        highlight(component3 != null ? focus.parent.parent : focus, color);
                                    }
                                }
                            }
                            else
                            {
                                target = null;
                                interactable = null;
                            }
                        }
                    }
                }
            }
            else
            {
                if (focus != null && PlayerInteract.interactable != null)
                {
                    InteractableDoorHinge component4 = focus.GetComponent<InteractableDoorHinge>();
                    HighlighterTool.unhighlight(component4 != null ? focus.parent.parent : focus);
                }

                focus = null;
                target = null;
                interactable = null;
                interactable2 = null;
            }

            if (Player.player.life.isDead) return;

            if (PlayerInteract.interactable != null)
            {
                if (PlayerInteract.interactable.checkHint(out EPlayerMessage message, out string text, out Color color2)
                    && !PlayerUI.window.showCursor)
                {
                    if (PlayerInteract.interactable.CompareTag("Item"))
                    {
                        PlayerUI.hint(!(target != null) ? focus : target, message, text, color2,
                            ((InteractableItem)PlayerInteract.interactable).item, ((InteractableItem)PlayerInteract.interactable).asset);
                    }
                    else
                        PlayerUI.hint(!(target != null) ? focus : target, message, text, color2);
                }
            }
            else if (purchaseAsset != null && Player.player.movement.purchaseNode != null && !PlayerUI.window.showCursor)
                PlayerUI.hint(null, EPlayerMessage.PURCHASE, string.Empty, Color.white, purchaseAsset.itemName,
                    Player.player.movement.purchaseNode.cost);
            else if (focus != null && focus.CompareTag("Enemy"))
            {
                Player player = DamageTool.getPlayer(focus);

                if (player != null && player != Player.player && !PlayerUI.window.showCursor)
                    PlayerUI.hint(null, EPlayerMessage.ENEMY, string.Empty, Color.white, player.channel.owner);
            }
            if (PlayerInteract.interactable2 != null && PlayerInteract.interactable2.checkHint(out EPlayerMessage message2,
                    out float data) && !PlayerUI.window.showCursor)
                PlayerUI.hint2(message2, !isHoldingKey ? 0f : (Time.realtimeSinceStartup - lastKeyDown) / salvageTime, data);

            if ((Player.player.stance.stance == EPlayerStance.DRIVING ||
                 Player.player.stance.stance == EPlayerStance.SITTING) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.F1))
                    hotkey(0);

                if (Input.GetKeyDown(KeyCode.F2))
                    hotkey(1);

                if (Input.GetKeyDown(KeyCode.F3))
                    hotkey(2);

                if (Input.GetKeyDown(KeyCode.F4))
                    hotkey(3);

                if (Input.GetKeyDown(KeyCode.F5))
                    hotkey(4);

                if (Input.GetKeyDown(KeyCode.F6))
                    hotkey(5);

                if (Input.GetKeyDown(KeyCode.F7))
                    hotkey(6);

                if (Input.GetKeyDown(KeyCode.F8))
                    hotkey(7);

                if (Input.GetKeyDown(KeyCode.F9))
                    hotkey(8);

                if (Input.GetKeyDown(KeyCode.F10))
                    hotkey(9);
            }

            if (Input.GetKeyDown(ControlsSettings.interact))
            {
                lastKeyDown = Time.realtimeSinceStartup;
                isHoldingKey = true;
            }

            if (Input.GetKeyDown(ControlsSettings.inspect) && ControlsSettings.inspect != ControlsSettings.interact &&
                Player.player.equipment.canInspect)
                Player.player.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);

            if (!isHoldingKey) return;

            if (Input.GetKeyUp(ControlsSettings.interact))
            {
                isHoldingKey = false;

                if (PlayerUI.window.showCursor)
                {
                    if (Player.player.inventory.isStoring)
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
                            PlayerNPCDialogueUI.skipText();
                        else if (PlayerNPCDialogueUI.dialogueHasNextPage)
                            PlayerNPCDialogueUI.nextPage();
                        else
                        {
                            PlayerNPCDialogueUI.close();
                            PlayerLifeUI.open();
                        }
                    }
                    else if (PlayerNPCQuestUI.active)
                        PlayerNPCQuestUI.closeNicely();
                    else if (PlayerNPCVendorUI.active)
                        PlayerNPCVendorUI.closeNicely();
                }
                else if (Player.player.stance.stance == EPlayerStance.DRIVING ||
                         Player.player.stance.stance == EPlayerStance.SITTING)
                    VehicleManager.exitVehicle();
                else if (focus != null && PlayerInteract.interactable != null)
                {
                    if (PlayerInteract.interactable.checkUseable())
                        PlayerInteract.interactable.use();
                }
                else if (purchaseAsset != null)
                {
                    if (Player.player.skills.experience >= Player.player.movement.purchaseNode.cost)
                        Player.player.skills.sendPurchase(Player.player.movement.purchaseNode);
                }
                else if (ControlsSettings.inspect == ControlsSettings.interact && Player.player.equipment.canInspect)
                    Player.player.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
            }
            else if (Time.realtimeSinceStartup - lastKeyDown > salvageTime)
            {
                isHoldingKey = false;

                if (!PlayerUI.window.showCursor && PlayerInteract.interactable2 != null)
                    PlayerInteract.interactable2.use();
            }
        }

        #endregion
    }
}
