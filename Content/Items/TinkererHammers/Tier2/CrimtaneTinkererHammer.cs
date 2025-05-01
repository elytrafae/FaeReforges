using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Cooldowns;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class CrimtaneTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(ModContent.GetInstance<CrimsonHammerCooldown>().DisplayCooldownTicks / 60f);
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetModPlayer<MyReforgeHammerPlayer>().crimtaneAccessoryCount++;
            
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.Shadewood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
