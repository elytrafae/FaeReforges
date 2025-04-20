using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class HallowedTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;
        public override int HammerTier => 3;
    
        // Effect is implemented somewhere else

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
