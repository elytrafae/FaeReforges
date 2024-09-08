using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammers {
    internal class ReforgeHammerEnhancedGlobalProjectile : GlobalProjectile {

        private Item? createdByItem = null;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            if (source is EntitySource_ItemUse itemUseSource) {
                createdByItem = itemUseSource.Item;
            } else if (source is IEntitySource_WithStatsFromItem statsSource) {
                createdByItem = statsSource.Item;
            } else if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                createdByItem = parentProj.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().createdByItem;
            }
            createdByItem?.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onCreateProjectile(createdByItem, projectile, source);
    }

        // The following is for melee hits only. For projectiles, see ReforgeHammerEnhancedGlobalProjectile
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            createdByItem?.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamageNpc(createdByItem, Main.player[projectile.owner], target, ref modifiers);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            createdByItem?.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.changeWeaponDealDamagePvp(createdByItem, Main.player[projectile.owner], target, ref modifiers);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            createdByItem?.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamageNpc(createdByItem, Main.player[projectile.owner], target, hit, damageDone);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            createdByItem?.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onWeaponDealDamagePvp(createdByItem, Main.player[projectile.owner], target, info);
        }

        public int GetHammerItemTypeOrNone() {
            if (createdByItem == null || createdByItem.type == ItemID.None) {
                return ItemID.None;
            }
            return createdByItem.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammerItemTypeOrNone();
        }

    }
}
