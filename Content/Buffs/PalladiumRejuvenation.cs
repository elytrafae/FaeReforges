using FaeLibrary.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class PalladiumRejuvenation : ModBuff, IFaeBuff {

        public void UpdateLifeRegen(Player player, ref int buffIndex) {
            player.lifeRegen += 3;
        }

    }
}
