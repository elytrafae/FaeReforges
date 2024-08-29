using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers {
    public class StoneTinkererHammer : ModItem {

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Gray;
            Item.maxStack = 1;
            Item.value = Terraria.Item.buyPrice(silver: 1);
        }

    }
}
