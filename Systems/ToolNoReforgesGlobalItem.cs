using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace FaeReforges.Systems {
    public class ToolNoReforgesGlobalItem : GlobalItem {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            return entity.pick > 0 || entity.axe > 0 || entity.hammer > 0;
        }

        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) {
            return false;
        }

        public override bool AllowPrefix(Item item, int pre) {
            return false;
        }

        public override float UseSpeedMultiplier(Item item, Player player) {
            return 1.15f;
        }

    }
}
