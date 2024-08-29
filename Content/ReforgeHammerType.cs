using FaeReforges.Content.Items;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace FaeReforges.Content {
    public class ReforgeHammerType {
        public LocalizedText WeaponEffect { get; private set; }
        public LocalizedText AccessoryEffect { get; private set; }

        internal void SetupLocalization(Item hammerItem) {
            string typeName = hammerItem.type.ToString();
            string modName = "";
            ModItem modItem = hammerItem.ModItem;
            if (modItem != null) {
                typeName = modItem.Name;
                modName = modItem.Mod.Name + ".";
            }
            WeaponEffect = Language.GetText($"{modName}{ReforgeHammerLocalization.LocalizationCategory}.{typeName}.{nameof(WeaponEffect)}");
            AccessoryEffect = Language.GetText($"{modName}{ReforgeHammerLocalization.LocalizationCategory}.{typeName}.{nameof(WeaponEffect)}");
        }

        // TODO: Add functionality to these
        public int negativeReforgeChance = 0;
        public int reforgeCost = 0;
        public Action<Item> onApplyWeapon = (item) => { };
        public Action<Item> onApplyAccessory = (item) => { };
        public Action<Item, Player> onUpdateWeaponHeld = (item, player) => { };
        public Action<Item, Player> onUpdateAccessory = (item, player) => { };
        public Action<Item, Player, Player, Player.HurtModifiers> changeWeaponDealDamagePvp = (item, attacker, victim, hurtModifiers) => { };
        public Action<Item, Player, Player, Player.HurtInfo> onWeaponDealDamagePvp = (item, attacker, victim, hurtInfo) => { };
        public Action<Item, Player, Player, NPC.HitModifiers> changeWeaponDealDamageNpc = (item, attacker, victim, hitModifiers) => { };
        public Action<Item, Player, Player, NPC.HitInfo> onWeaponDealDamageNpc = (item, attacker, victim, hitInfo) => { };

    }
}
