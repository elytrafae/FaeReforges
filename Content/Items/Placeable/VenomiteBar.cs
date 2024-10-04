using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.Placeable
{

    // Disabled content!
    public abstract class VenomiteBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 63; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

            // The Chlorophyte Extractinator can exchange items. Here we tell it to allow a one-way exchanging of 5 ExampleBar for 2 ChlorophyteBar.
            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.ChlorophyteBar, 1);
        }

        public override void SetDefaults()
        {
            // ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.VenomiteBar>());
            Item.width = 20;
            Item.height = 20;
            Item.value = Terraria.Item.buyPrice(gold: 1);
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar)
                .AddIngredient(ItemID.VialofVenom)
                .AddIngredient(ItemID.SpiderFang, 2)
                .AddTile(TileID.ImbuingStation)
                .Register();
        }

    }
}
