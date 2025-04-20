using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.VanillaReforges;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class TinTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement++; 
            player.jumpSpeedBoost *= 0.01f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TinBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
