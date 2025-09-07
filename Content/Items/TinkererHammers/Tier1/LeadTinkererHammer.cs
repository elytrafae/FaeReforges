using FaeLibrary.API.ItemConditions;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class LeadTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.ThisButNotThis(ItemCondition.IsWeapon, ItemCondition.GrammaticalAnd(ItemCondition.IsMinionWeapon, ItemCondition.IsSentryWeapon));
        public override void HammerModifyWeaponCrit(Entity itemOrProjectile, Player player, ref float crit) {
            crit += 5;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

}
