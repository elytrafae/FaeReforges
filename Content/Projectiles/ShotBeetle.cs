using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Projectiles {
    public class ShotBeetle : ModProjectile {

        public override void SetStaticDefaults() {
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.GiantBee);
            Projectile.ArmorPenetration = 25;
            Projectile.penetrate = 5;
            Projectile.width = 22;
            Projectile.height = 17;
            AIType = ProjectileID.GiantBee;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }

    }
}
