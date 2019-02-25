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
            MiscOptions.CustomSalvageTime ? MiscOptions.SalvageTime : OptimizationVariables.MainPlayer.channel.owner.isAdmin ? 1f : 8f;

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
        
        public static void highlight(Transform target, Color color)
        {
            if (target.CompareTag("Player") || target.CompareTag("Enemy") || target.CompareTag("Zombie") || target.CompareTag("Animal") || target.CompareTag("Agent"))
                return;
            Highlighter highlighter = target.GetComponent<Highlighter>();
            if (highlighter == null)
                highlighter = target.gameObject.AddComponent<Highlighter>();
            highlighter.ConstantOn(color);
        }

        [OnSpy]
        public static void OnSpied()
        {
            Transform cPos = OptimizationVariables.MainCam.transform;
            PhysicsUtility.raycast(new Ray(cPos.position, cPos.forward), out hit, OptimizationVariables.MainPlayer.look.perspective == EPlayerPerspective.THIRD ? 6 : 4, RayMasks.PLAYER_INTERACT, 0);
        }

        [Override(typeof(PlayerInteract), "Update", BindingFlags.NonPublic | BindingFlags.Instance)]
        public void OV_Update() // i have no idea what any of this does tbh
        {
            if (!DrawUtilities.ShouldRun())
                return;

            if (OptimizationVariables.MainPlayer.stance.stance != EPlayerStance.DRIVING && OptimizationVariables.MainPlayer.stance.stance != EPlayerStance.SITTING &&
                !OptimizationVariables.MainPlayer.life.isDead && !OptimizationVariables.MainPlayer.workzone.isBuilding)
            {
                if (Time.realtimeSinceStartup - lastInteract > 0.1f)
                {
                    int Mask = 0;

                    if (InteractionOptions.InteractThroughWalls && !PlayerCoroutines.IsSpying)
                    {
                        if (!InteractionOptions.NoHitBarricades)
                            Mask |= RayMasks.BARRICADE;

                        if (!InteractionOptions.NoHitItems)
                            Mask |= RayMasks.ITEM;

                        if (!InteractionOptions.NoHitResources)
                            Mask |= RayMasks.RESOURCE;

                        if (!InteractionOptions.NoHitStructures)
                            Mask |= RayMasks.STRUCTURE;

                        if (!InteractionOptions.NoHitVehicles)
                            Mask |= RayMasks.VEHICLE;

                        if (!InteractionOptions.NoHitEnvironment)
                            Mask |= RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND;
                    }
                    else
                        Mask = RayMasks.PLAYER_INTERACT;
                    
                    lastInteract = Time.realtimeSinceStartup;

                    bool Run = InteractionOptions.InteractThroughWalls && !PlayerCoroutines.IsSpying;
                    float Range = Run ? 20f : 4f;

                    PlayerLook pLook = OptimizationVariables.MainPlayer.look;
                    PhysicsUtility.raycast(new Ray(pLook.aim.position, pLook.aim.forward), out hit, OptimizationVariables.MainPlayer.look.perspective == EPlayerPerspective.THIRD ? Range + 2 : Range, Mask, 0);
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

            if (OptimizationVariables.MainPlayer.life.isDead) return;

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
            else if (purchaseAsset != null && OptimizationVariables.MainPlayer.movement.purchaseNode != null && !PlayerUI.window.showCursor)
                PlayerUI.hint(null, EPlayerMessage.PURCHASE, string.Empty, Color.white, purchaseAsset.itemName,
                    OptimizationVariables.MainPlayer.movement.purchaseNode.cost);
            else if (focus != null && focus.CompareTag("Enemy"))
            {
                Player player = DamageTool.getPlayer(focus);

                if (player != null && player != OptimizationVariables.MainPlayer && !PlayerUI.window.showCursor)
                    PlayerUI.hint(null, EPlayerMessage.ENEMY, string.Empty, Color.white, player.channel.owner);
            }
            if (PlayerInteract.interactable2 != null && PlayerInteract.interactable2.checkHint(out EPlayerMessage message2,
                    out float data) && !PlayerUI.window.showCursor)
                PlayerUI.hint2(message2, !isHoldingKey ? 0f : (Time.realtimeSinceStartup - lastKeyDown) / salvageTime, data);

            if ((OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.DRIVING ||
                 OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.SITTING) && !Input.GetKey(KeyCode.LeftShift))
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
                OptimizationVariables.MainPlayer.equipment.canInspect)
                OptimizationVariables.MainPlayer.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);

            if (!isHoldingKey) return;

            if (Input.GetKeyUp(ControlsSettings.interact))
            {
                isHoldingKey = false;

                if (PlayerUI.window.showCursor)
                {
                    if (OptimizationVariables.MainPlayer.inventory.isStoring)
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
                else if (OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.DRIVING ||
                         OptimizationVariables.MainPlayer.stance.stance == EPlayerStance.SITTING)
                    VehicleManager.exitVehicle();
                else if (focus != null && PlayerInteract.interactable != null)
                {
                    if (PlayerInteract.interactable.checkUseable())
                        PlayerInteract.interactable.use();
                }
                else if (purchaseAsset != null)
                {
                    if (OptimizationVariables.MainPlayer.skills.experience >= OptimizationVariables.MainPlayer.movement.purchaseNode.cost)
                        OptimizationVariables.MainPlayer.skills.sendPurchase(OptimizationVariables.MainPlayer.movement.purchaseNode);
                }
                else if (ControlsSettings.inspect == ControlsSettings.interact && OptimizationVariables.MainPlayer.equipment.canInspect)
                    OptimizationVariables.MainPlayer.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
            }
            else if (Time.realtimeSinceStartup - lastKeyDown > salvageTime)
            {
                isHoldingKey = false;

                if (!PlayerUI.window.showCursor && PlayerInteract.interactable2 != null)
                    PlayerInteract.interactable2.use();
            }
        }
    }
}
