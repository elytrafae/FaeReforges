using FaeLibrary.API.ItemConditions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class IronTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 3);
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            damage *= 1.05f;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
