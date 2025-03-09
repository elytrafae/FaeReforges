using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {
    internal class MyWeaponImbuePlayer : ModPlayer {

        public const int FROSTBURN_DURATION = 6 * 60;

        public bool frostburn = false;

        public override void ResetEffects() {
            frostburn = false;
        }

        public static MyWeaponImbuePlayer Get(Player player) { 
            return player.GetModPlayer<MyWeaponImbuePlayer>();
        }

    }

    internal class MyWeaponImbueItem : GlobalItem {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            if (MyWeaponImbuePlayer.Get(player).frostburn) {
                target.AddBuff(BuffID.Frostburn, MyWeaponImbuePlayer.FROSTBURN_DURATION);
            }
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            if (MyWeaponImbuePlayer.Get(player).frostburn) {
                target.AddBuff(BuffID.Frostburn, MyWeaponImbuePlayer.FROSTBURN_DURATION);
            }
        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
            Vector2 position = new(hitbox.X, hitbox.Y);
            int width = hitbox.Width;
            int height = hitbox.Height;
            if (MyWeaponImbuePlayer.Get(player).frostburn && Main.rand.NextBool(5)) {
                Dust.NewDust(position, width, height, DustID.Ice);
            }
        }

    }

    internal class MyWeaponImbueProjectile : GlobalProjectile {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            if (projectile.TryGetOwner(out Player player)) {
                if (MyWeaponImbuePlayer.Get(player).frostburn) {
                    target.AddBuff(BuffID.Frostburn, MyWeaponImbuePlayer.FROSTBURN_DURATION);
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            if (projectile.TryGetOwner(out Player player)) {
                if (MyWeaponImbuePlayer.Get(player).frostburn) {
                    target.AddBuff(BuffID.Frostburn, MyWeaponImbuePlayer.FROSTBURN_DURATION);
                }
            }
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 position, int width, int height) {
            if (projectile.TryGetOwner(out Player player)) {
                if (MyWeaponImbuePlayer.Get(player).frostburn && Main.rand.NextBool(5)) {
                    Dust.NewDust(position, width, height, DustID.Ice);
                }
            }
        }
    }
}
