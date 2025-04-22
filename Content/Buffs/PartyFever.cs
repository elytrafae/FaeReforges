using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class PartyFever : ModBuff {

        public const float OUTGOING_DAMAGE_MULTIPLIER = 0.9f;

        public override void SetStaticDefaults() {
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetDamage(DamageClass.Generic) *= OUTGOING_DAMAGE_MULTIPLIER;
        }

    }
}
