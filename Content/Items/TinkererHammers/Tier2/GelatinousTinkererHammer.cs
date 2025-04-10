using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Cooldowns;
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
    internal class GelatinousTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 45);
        public override int HammerTier => 2;

        public override LocalizedText AccessoryEffectText => base.AccessoryEffectText.WithFormatArgs(ModContent.GetInstance<GelatinousHammerCooldown>().DisplayCooldownTicks/60f, ModContent.GetInstance<GelatinousHammerCooldown>().Charges);

        public override ItemCondition ReforgeableCondition => ItemCondition.Any;

        public override void HammerModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback) {
            knockback += 0.3f;
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count == 3) {
                if (ModContent.GetInstance<GelatinousHammerCooldown>().CanBeUsed()) {
                    player.dashType = DashID.CrystalAssassin;
                }
                if (player.dashDelay == 1) {
                    ModContent.GetInstance<GelatinousHammerCooldown>().ConsumeCharge();
                }
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.GelBalloon, 20)
                .AddIngredient(ItemID.SillyBalloonPink, 5)
                .AddIngredient(ItemID.SillyBalloonPurple, 5)
                .AddIngredient(ItemID.SillyBalloonGreen, 5)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }

    }
}
