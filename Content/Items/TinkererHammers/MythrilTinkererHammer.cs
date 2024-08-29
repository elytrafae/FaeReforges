using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FaeReforges.Content.Items.TinkererHammers {
    internal class MythrilTinkererHammer : ModItem {

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 1;
            Item.value = Terraria.Item.buyPrice(gold: 1, silver: 30);
        }

    }
}
