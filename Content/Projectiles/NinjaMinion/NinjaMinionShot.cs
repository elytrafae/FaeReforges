using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Projectiles.NinjaMinion {
    public class NinjaMinionShot : ModProjectile {

        public override void SetStaticDefaults() {
            ProjectileID.Sets.MinionShot[Type] = true;
        }

        public override void SetDefaults() {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 5;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ArmorPenetration = 20;
        }

        public override void AI() {
            Projectile.rotation += 0.01f;
        }

    }
}
