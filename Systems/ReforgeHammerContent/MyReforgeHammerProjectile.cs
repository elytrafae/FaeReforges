using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {
    public class MyReforgeHammerProjectile : GlobalProjectile {

        private bool isBoostedByForbidden = false;
        private bool isBoostedByForbiddenInherited = false;

        public override bool InstancePerEntity => true;

        // Called in the Minion Speed IL place
        public bool UpdatePreAI(Projectile projectile) {
            if ((projectile.minion || projectile.sentry) && projectile.TryGetOwner(out Player player) &&
                projectile.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().GetHammerItemTypeOrNone() == ModContent.ItemType<ForbiddenTinkererHammer>()) {
                int reduceManaAmount = (int)(40 * projectile.minionSlots);
                isBoostedByForbidden = reduceManaAmount > 0 && player.statManaMax2 >= reduceManaAmount;
                if (isBoostedByForbidden) {
                    player.statManaMax2 -= reduceManaAmount;
                    player.statMana = Math.Min(player.statLifeMax2, player.statMana);
                    //for (int i = 0; i < 5; i++) {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Sand);
                    //}
                }
            } else {
                isBoostedByForbidden = false;
            }
            return true;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                MyReforgeHammerProjectile parent = parentProj.GetGlobalProjectile<MyReforgeHammerProjectile>();
                isBoostedByForbiddenInherited = parent.isBoostedByForbidden || parent.isBoostedByForbiddenInherited;
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            if (isBoostedByForbidden || isBoostedByForbiddenInherited) {
                modifiers.FinalDamage *= 1.15f;
            }
        }


        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            if (isBoostedByForbidden || isBoostedByForbiddenInherited) {
                modifiers.FinalDamage *= 1.15f;
            }
        }

    }
}
