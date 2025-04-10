using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    internal class General : ModBuff {

        const int MAX_STACK_TIME = 20 * 60;

        public override void SetStaticDefaults() {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            player.buffTime[buffIndex] += time;
            if (player.buffTime[buffIndex] > MAX_STACK_TIME) {
                player.buffTime[buffIndex] = MAX_STACK_TIME;
            }
            return true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.maxMinions += 1;
            player.maxTurrets += 1;
        }

    }
}
