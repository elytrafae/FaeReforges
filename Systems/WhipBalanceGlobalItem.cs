using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    public class WhipBalanceGlobalItem : GlobalItem {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            return entity.DamageType == DamageClass.SummonMeleeSpeed;
        }

        // Since we enabled crits, we make the whips worse in damage
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            damage *= 0.9f;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
            if (item.type == ItemID.BlandWhip) {
                crit += 4;
            }
        }

    }
}
