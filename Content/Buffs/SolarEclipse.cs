using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammerContent;
using FaeReforges.Systems.ReforgeHammers;
using FaeReforges.Systems.VanillaReforges;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    internal class SolarEclipse : ModBuff {

        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
            Player p = Main.player[Main.myPlayer];
            return p.GetModPlayer<MyReforgeHammerPlayer>().accessoryReforgedWithSolar;
            return ReforgeHammerUtility.GetHammerItemType(p.HeldItem) == ModContent.ItemType<SolarTinkererHammer>();
        }
    }
}
