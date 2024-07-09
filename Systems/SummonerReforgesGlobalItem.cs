using FaeReforges.Content.Reforges;
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
    internal class SummonerReforgesGlobalItem : GlobalItem {

        public float minionOccupancyMult = 1f;
        public float minionSpeedMult = 1f;
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            return ItemID.Sets.StaffMinionSlotsRequired[entity.type] > 0;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand) {
            return rand.NextFromCollection<AbstractSummonerPrefix>(ModContent.GetContent<AbstractSummonerPrefix>().ToList<AbstractSummonerPrefix>()).Type;
        }

    }
}
