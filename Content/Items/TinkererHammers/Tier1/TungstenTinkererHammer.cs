using FaeLibrary.API.ItemConditions;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class TungstenTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 6);
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd(ItemCondition.IsMagicWeapon, ItemCondition.IsAccessory);
        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.manaCost -= 0.02f;
        }

        public override float HammerUseSpeedMultiplier(Item item, Player player) {
            return 0.92f; // 8% faster
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
