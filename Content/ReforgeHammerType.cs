using FaeReforges.Content.Items;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace FaeReforges.Content {
    public class ReforgeHammerType {
        public LocalizedText WeaponEffect { get; private set; }
        public LocalizedText AccessoryEffect { get; private set; }

        internal void SetupLocalization(Item hammerItem) {
            string typeName = "Item" + hammerItem.type.ToString();
            string modName = ReforgeHammerLocalization.LocalizationCategory;
            ModItem modItem = hammerItem.ModItem;
            if (modItem != null) {
                typeName = modItem.Name;
                modName = modItem.Mod.GetLocalizationKey(ReforgeHammerLocalization.LocalizationCategory);
            }
            WeaponEffect = Language.GetOrRegister($"{modName}.{typeName}.{nameof(WeaponEffect)}", () => "");
            AccessoryEffect = Language.GetOrRegister($"{modName}.{typeName}.{nameof(AccessoryEffect)}", () => "");
        }

        //public int negativeReforgeChance = 0;
        //public int reforgeCost = 0;
        public Action<Item> onApplyWeapon = (item) => { };
        public Action<Item> onApplyAccessory = (item) => { };
        public Action<Item, Player> onUpdateWeaponHeld = (item, player) => { };
        public Action<Item, Player, bool> onUpdateAccessory = (item, player, hideVisual) => { };
        public PassFourthParameterAsRefAction<int, Player, Player, Player.HurtModifiers> changeWeaponDealDamagePvp = (int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) => { };
        public Action<int, Player, Player, Player.HurtInfo> onWeaponDealDamagePvp = (item, attacker, victim, hurtInfo) => { };
        public PassFourthParameterAsRefAction<int, Player, NPC, NPC.HitModifiers> changeWeaponDealDamageNpc = (int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => { };
        public Action<int, Player, NPC, NPC.HitInfo, int> onWeaponDealDamageNpc = (item, attacker, victim, hitInfo, damageDone) => { };
        public Func<Item, Player, bool> canUseItem = (item, attacker) => { return true; };
        public Action<int, Projectile, IEntitySource> onCreateProjectile = (item, projectile, source) => { };


        public delegate void PassFourthParameterAsRefAction<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 arg3, ref T4 arg4);
        public delegate void PassThirdParameterAsRefAction<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, ref T4 arg3);
    }
}
