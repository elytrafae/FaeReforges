using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Projectiles {
    public class FriendlyStinger : ModProjectile {

        // "Why not use the stinger projectile from the hornet minion?"
        // That one is marked for summoner

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Stinger);
            AIType = ProjectileID.Stinger;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Default;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.Poisoned, 6 * 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(BuffID.Poisoned, 6 * 60);
        }

    }
}
