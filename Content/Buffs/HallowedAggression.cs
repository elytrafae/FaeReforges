using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class HallowedAggression : ModBuff {

        public override void SetStaticDefaults() {
            
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetCritChance(DamageClass.Generic) += 10;
            player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement += 10; // Once again, too lazy
        }

    }
}
