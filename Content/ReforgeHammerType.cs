using FaeReforges.Content.ItemConditions;
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

        public ReforgeHammerType(int tier) {
            hammerTier = tier;
        }

        public int hammerTier;
        public ItemCondition reforgableCondition = ItemCondition.Any;
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

        public PassThirdParameterAsRefAction<Item, Player, StatModifier> modifyWeaponDamage = (Item item, Player player, ref StatModifier damage) => { };
        public PassThirdParameterAsRefAction<Item, Player, float> modifyWeaponCrit = (Item item, Player player, ref float crit) => { };
        public PassThirdParameterAsRefAction<Item, Player, StatModifier> modifyWeaponKnockback = (Item item, Player player, ref StatModifier knockback) => { };
        public ModifyShootStatsAction modifyShootStats = (Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => { };
        public PassThirdAndFourthParameterAsRefAction<Item, Player, float, float> modifyManaCost = (Item item, Player player, ref float reduce, ref float mult) => {};
        public PassThirdParameterAsRefAction<Item, Player, float> modifyItemScale = (Item item, Player player, ref float scale) => { };
        public Func<Item, Player, float> useSpeedMultiplier = (Item item, Player player) => 1f;

        public delegate void PassFourthParameterAsRefAction<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 arg3, ref T4 arg4);
        public delegate void PassThirdParameterAsRefAction<in T1, in T2, T3>(T1 arg1, T2 arg2, ref T3 arg3);
        public delegate void PassThirdAndFourthParameterAsRefAction<in T1, in T2, T3, T4>(T1 arg1, T2 arg2, ref T3 arg3, ref T4 arg4);
        public delegate void ModifyShootStatsAction(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback);
    }
}
