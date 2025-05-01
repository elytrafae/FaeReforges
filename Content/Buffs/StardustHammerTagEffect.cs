using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class StardustHammerTagEffect : ModBuff {

        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

    }
}
