using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeReforges.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            if (Main.rand.NextBool(7)) {
                SpawnConfetti(player, player.GetSource_Buff(buffIndex));
            }
        }

        public override void Update(NPC npc, ref int buffIndex) {
            // Outgoing damage reduction somewhere else
            if (Main.rand.NextBool(7)) {
                SpawnConfetti(npc, npc.GetSource_Buff(buffIndex));
            }
        }

        public static void SpawnConfetti(Entity entity, IEntitySource source) {
            Dust.NewDust(entity.position, entity.width, entity.height, DustID.Confetti + Main.rand.Next(4));
            Gore.NewGore(source, entity.position + new Vector2(Main.rand.Next(entity.width * 3 / 4), Main.rand.Next(entity.height * 3 / 4)), new Vector2(0, -0.8f), 276 + Main.rand.Next(7));
        }

    }
}
