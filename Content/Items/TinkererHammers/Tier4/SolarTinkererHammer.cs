using FaeLibrary.API.ClassExtensions;
using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    internal class SolarTinkererHammer : SimpleTinkererHammerItem {

        public const float ENDURANCE_BONUS = 0.01f;
        public const float MELEE_SCALE_BONUS = 0.04f;

        public const int WEAPON_POTION_SICKNESS = 15 * 60;
        public const int WEAPON_REGEN = 3;
        public const float WEAPON_SPEED_BONUS = 0.15f;
        public const float WEAPON_SIZE_BONUS = 0.15f;

        public override int Rarity => ItemRarityID.Red;
        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsMeleeWeapon, ItemCondition.IsAccessory);
        public override int HammerTier => 4;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(ENDURANCE_BONUS*100, MELEE_SCALE_BONUS*100);
        }

        public override string GetWeaponEffectText(Item item) {
            return WeaponEffectText.Format(
                WEAPON_REGEN / 2f, 
                (WEAPON_SPEED_BONUS * 100).ToString("#0.##"), 
                (WEAPON_SIZE_BONUS * 100).ToString("#0.##"), 
                WEAPON_POTION_SICKNESS / 60f
            );
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.endurance += ENDURANCE_BONUS;
            player.GetItemSizeStat(DamageClass.Melee) += MELEE_SCALE_BONUS;
        }

        public override float HammerUseSpeedMultiplier(Item item, Player player) {
            return 1f + WEAPON_SPEED_BONUS;
        }

        public override void HammerModifyItemScale(Item item, Player player, ref float scale) {
            scale += WEAPON_SIZE_BONUS;
        }

        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            player.AddBuff(BuffID.PotionSickness, WEAPON_POTION_SICKNESS);
            player.GetModPlayer<MyReforgeHammerPlayer>().commonPositiveRegen += WEAPON_REGEN;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }

    }
}
