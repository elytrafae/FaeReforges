using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Steamworks;

namespace FaeReforges.Systems {
    internal class SummonTagChangesSystem : ModSystem {

        public override void Load() {
            On_Projectile.Damage += On_Projectile_Damage;
        }

        private void On_Projectile_Damage(On_Projectile.orig_Damage orig, Projectile self) {
            if (self.TryGetGlobalProjectile(out SummonerReforgesGlobalProjectile globProj)) {
                float oldNr = ProjectileID.Sets.SummonTagDamageMultiplier[self.type];
                ProjectileID.Sets.SummonTagDamageMultiplier[self.type] = oldNr * globProj.bonusTagEffectiveness;
                try {
                    orig(self);
                } catch {
                    // Ignore errors IG
                }
                ProjectileID.Sets.SummonTagDamageMultiplier[self.type] = oldNr;
            } else {
                orig(self);
            }
        }

    }
}
