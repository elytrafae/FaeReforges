using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.GoblinTinkerer {
    public class GoblinTinkererGlobalNPC : GlobalNPC {

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
            return entity.type == NPCID.GoblinTinkerer;
        }

         

    }
}
