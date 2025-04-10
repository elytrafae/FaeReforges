using FaeReforges.Content.Items.TinkererHammers.Tier2;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ShopsAndLoot {
    internal class ShopGlobalNPC : GlobalNPC {

        public override void ModifyShop(NPCShop shop) {
            if (shop.NpcType == NPCID.DD2Bartender) {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<EternianTinkererHammer>(), Condition.DownedOldOnesArmyT1));

                shop.Add(new NPCShop.Entry(ModContent.ItemType<SquireTinkererHammer>(), Condition.DownedOldOnesArmyT2));
                shop.Add(new NPCShop.Entry(ModContent.ItemType<HuntressTinkererHammer>(), Condition.DownedOldOnesArmyT2));
                shop.Add(new NPCShop.Entry(ModContent.ItemType<ApprenticeTinkererHammer>(), Condition.DownedOldOnesArmyT2));
                shop.Add(new NPCShop.Entry(ModContent.ItemType<MonkTinkererHammer>(), Condition.DownedOldOnesArmyT2));
            }
        }

    }
}
