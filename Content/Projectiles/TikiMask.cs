using FaeReforges.Content.Items.TinkererHammers.Tier4;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Projectiles {
    public class TikiMask : ModProjectile {

        public Item summonedByItem = null;

        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            Projectile.width = 28; 
            Projectile.height = 48;
            Projectile.ContinuouslyUpdateDamageStats = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 4;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override bool? CanCutTiles() {
            return false;
        }

        public override void AI() {
            Projectile.timeLeft = 4;
            if (Projectile.owner == Main.myPlayer) {
                if (Projectile.TryGetOwner(out Player player)) {
                    if (summonedByItem == null || summonedByItem != player.HeldItem) { 
                        Projectile.Kill();
                        return;
                    }
                    Projectile.ai[0] = Main.MouseWorld.X;
                    Projectile.ai[1] = Main.MouseWorld.Y;

                    if (Math.Abs(Projectile.ai[0] - Projectile.localAI[0]) >= 16 || Math.Abs(Projectile.ai[1] - Projectile.localAI[1]) >= 16) {
                        Projectile.localAI[0] = Projectile.ai[0];
                        Projectile.localAI[1] = Projectile.ai[1];
                        Projectile.netUpdate = true;
                    }
                }
            }
            Projectile.velocity.X = (Projectile.ai[0] - Projectile.Center.X) / 10;
            Projectile.velocity.Y = (Projectile.ai[1] - Projectile.Center.Y) / 10;
            Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 0.5f, 0.25f));
        }

    }
}
