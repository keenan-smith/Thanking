using SDG.Provider;
using SDG.Unturned;
using System.Collections.Generic;
using Thinking.Attributes;
using Thinking.Misc;
using UnityEngine;

namespace Thinking.Options
{
    public class Skin
    {
        public string Name;
        public int ID;

        public Skin(string Name, int ID)
        {
            this.Name = Name;
            this.ID = ID;
        }
    }

    public class WeaponSave
    {
        public ushort WeaponID;
        public int SkinID;

        public WeaponSave(ushort WeaponID, int SkinID)
        {
            this.WeaponID = WeaponID;
            this.SkinID = SkinID;
        }
    }

    public class SkinOptionList
    {
        public ESkinType Type = ESkinType.WEAPONS;
        public HashSet<Skin> Skins = new HashSet<Skin>();

        public SkinOptionList(ESkinType Type)
        {
            this.Type = Type;
        }
    }

    public static class SkinOptions
    {
        [Save] public static SkinConfig SkinConfig = new SkinConfig();
        public static SkinOptionList SkinWeapons = new SkinOptionList(ESkinType.WEAPONS);
        public static SkinOptionList SkinClothesShirts = new SkinOptionList(ESkinType.SHIRTS);
        public static SkinOptionList SkinClothesPants = new SkinOptionList(ESkinType.PANTS);
        public static SkinOptionList SkinClothesBackpack = new SkinOptionList(ESkinType.BACKPACKS);
        public static SkinOptionList SkinClothesVest = new SkinOptionList(ESkinType.VESTS);
        public static SkinOptionList SkinClothesHats = new SkinOptionList(ESkinType.HATS);
        public static SkinOptionList SkinClothesMask = new SkinOptionList(ESkinType.MASKS);
        public static SkinOptionList SkinClothesGlasses = new SkinOptionList(ESkinType.GLASSES);
    }

    public class SkinConfig
    {
        public HashSet<WeaponSave> WeaponSkins = new HashSet<WeaponSave>();
        public int ShirtID;
        public int PantsID;
        public int BackpackID;
        public int VestID;
        public int HatID;
        public int MaskID;
        public int GlassesID;
    }

    public enum ESkinType
    {
        NONE,
        WEAPONS,
        SHIRTS,
        PANTS,
        BACKPACKS,
        VESTS,
        HATS,
        MASKS,
        GLASSES
    }
}
