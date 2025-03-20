using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class AdamantiteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 68);
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerOnCreateProjectile(int item, Projectile projectile, IEntitySource source) {
            if (projectile.penetrate > -1) { // If the projectile doesn't have infinite pierce . . .
                projectile.penetrate += projectile.penetrate / 2; // 50% more penetrations, rounded down
                projectile.maxPenetrate = projectile.penetrate;
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
