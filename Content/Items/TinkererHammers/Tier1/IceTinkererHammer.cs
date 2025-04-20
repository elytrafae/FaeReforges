using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier1 {
    public class IceTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 1;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count == 4) { 
                MyWeaponImbuePlayer.Get(player).frostburn = true; 
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.IceTorch, 20)
                .AddIngredient(ItemID.FlinxFur, 4)
                .AddIngredient(ItemID.Shiverthorn, 10)
                .AddIngredient(ItemID.BorealWood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

}
