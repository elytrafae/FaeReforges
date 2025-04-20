using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using FaeLibrary.API.ItemConditions;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    public class TinkererHammerOfMight : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int HammerTier => 3;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetDamage(DamageClass.Generic) -= 0.01f;
            MyReforgeHammerPlayer2.Get(player).hammerOfMightCount++;
        }


        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
