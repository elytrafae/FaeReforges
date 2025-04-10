using FaeLibrary.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class MushroomMending : ModBuff, IFaeBuff {

        public override void SetStaticDefaults() {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetDamage(DamageClass.Generic) *= 0.7f;
        }

        public void UpdateLifeRegen(Player player, ref int buffIndex) {
            player.lifeRegen += (player.statLifeMax2 * 8 / 100); // 4% of Max HP per second
        }

    }
}
