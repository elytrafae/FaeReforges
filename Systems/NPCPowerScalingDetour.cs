using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    internal class NPCPowerScalingDetour : ModSystem {

        public override void Load() {
            IL_NPC.ScaleStats += IL_NPC_ScaleStats;
        }

        private void IL_NPC_ScaleStats(MonoMod.Cil.ILContext il) {
            ILCursor cursor = new ILCursor(il);

            cursor.GotoNext(inst => inst.MatchStfld<NPC>("life"));
            cursor.Index--;
            cursor.EmitLdarg0();
            cursor.EmitDelegate(ScaleStats);
        }

        public void ScaleStats(NPC npc) {
            double scale = 1f;
            // Add post moon lord bosses, like DOG here
            if (NPC.downedMoonlord) {
                scale = 1.15f;
            } else if (NPC.downedPlantBoss) {
                scale = 1.1f;
            } else if (Main.hardMode) {
                scale = 1.05f;
            }
            npc.lifeMax = (int)(npc.lifeMax * scale);
            npc.defense = (int)(npc.defense * scale);
            npc.damage = (int)(npc.damage * scale);
        } 
    }

}
