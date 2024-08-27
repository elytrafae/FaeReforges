using FaeReforges.Content.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.Placeable {
    public class TinkererAnvil : ModItem {

        public override void SetDefaults() {
            // ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TinkererAnvil>());
            Item.width = 28; // The item texture's width
            Item.height = 14; // The item texture's height
            Item.value = 150;
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        /*
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.WorkBench)
                .AddIngredient<ExampleItem>(10)
                .Register();
        }
        */

    }
}
