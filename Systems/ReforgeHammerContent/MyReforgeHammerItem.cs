using FaeReforges.Content.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammerContent {
    public class MyReforgeHammerItem : GlobalItem {

        /*
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
            if (MyReforgeHammerPlayer2.Get(player).shroomiteReforgeActive) {
                if (item.type == ItemID.Mushroom) {
                    healValue = 0;
                }
            }
        }
        */

        public override void OnConsumeItem(Item item, Player player) {
            if (MyReforgeHammerPlayer2.Get(player).shroomiteReforgeActive) {
                if (item.type == ItemID.Mushroom) {
                    player.AddBuff(ModContent.BuffType<MushroomMending>(), 15 * 60);
                }
            }
        }

    }
}
