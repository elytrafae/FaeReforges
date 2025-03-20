using FaeReforges.Content.Items.TinkererHammers.Tier2;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems {

    // This is for the adamantite hammer's "reduce damage loss from hitting enemies" effect
    internal class ProjectileDamageDetour : ModSystem {

        public override void Load() {
            On_Projectile.Damage += On_Projectile_Damage;
        }

        private void On_Projectile_Damage(On_Projectile.orig_Damage orig, Projectile self) {
            if (self.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().GetHammerItemTypeOrNone() == ModContent.ItemType<AdamantiteTinkererHammer>()) {
                int startDamage = self.damage;
                orig(self);
                int endDamage = self.damage;
                if (startDamage > endDamage) {
                    self.damage = endDamage + (startDamage - endDamage) / 2;
                }
            } else {
                orig(self);
            }
        }
    }
}
