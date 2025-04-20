using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FaeReforges.Content.Items.TinkererHammers.Tier2 {
    public class HoneycombTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int HammerTier => 2;
        public override ItemCondition ReforgeableCondition => ItemCondition.ThisButNotThis(ItemCondition.IsWeapon, ItemCondition.GrammaticalAnd(ItemCondition.IsMinionWeapon, ItemCondition.IsSentryWeapon));
        public override LocalizedText WeaponEffectText => base.WeaponEffectText.WithFormatArgs(ModContent.GetInstance<HoneycombHammerCooldown>().DisplayCooldownTicks/60f);

        public override void HammerWhileUsingWeapon(Item item, Player player) {
            BeeSwarm(item, player);
        }

        public override bool? HammerUseItem(Item item, Player player) {
            BeeSwarm(item, player); // Attempt to fix longswords
            return null;
        }

        private void BeeSwarm(Item item, Player player) {
            if (player.whoAmI == Main.myPlayer) {
                if (ModContent.GetInstance<HoneycombHammerCooldown>().ConsumeCharge()) {
                    DamageClass dmgClass = item.DamageType;

                    for (int i = 0; i < 5; i++) { // Spawn a swarm of 5 bees
                        Vector2 randomness = new(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)); // The last number is exclusive, so this produces an int within the range [-10, 10]
                        Vector2 spawnPos = player.Center + randomness;

                        Vector2 velocity = Main.MouseWorld - spawnPos;
                        velocity.Normalize();
                        velocity *= 6;

                        int damage = (int)player.GetDamage(dmgClass).ApplyTo(player.beeDamage(20));
                        float kb = player.GetKnockback(dmgClass).ApplyTo(player.beeKB(1));

                        Projectile proj = Projectile.NewProjectileDirect(item.GetSource_FromThis(), spawnPos, velocity, player.beeType(), damage, kb, player.whoAmI);
                        if (proj != null) {
                            proj.DamageType = dmgClass;
                        }
                    }

                }
            }
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.BeeWax, 10)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
}
