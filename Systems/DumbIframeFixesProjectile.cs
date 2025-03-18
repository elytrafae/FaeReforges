using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {

    // NOTE: Does not include minions. That is separate
    internal class DumbIframeFixesProjectile : GlobalProjectile {

        public override void SetDefaults(Projectile entity) {
            if (entity.type == ProjectileID.Bee || entity.type == ProjectileID.GiantBee) {
                entity.usesOwnerMeleeHitCD = false;
                entity.usesIDStaticNPCImmunity = true;
                entity.idStaticNPCHitCooldown = 10; // IDk how many frames bees typically inflict, but this is the default global iframe time
            }
        }

    }
}
