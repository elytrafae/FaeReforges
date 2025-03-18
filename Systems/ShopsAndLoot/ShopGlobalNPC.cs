using FaeReforges.Content.Items.TinkererHammers.Tier2;
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
                NPCShop.Entry eternianHammer = new(ModContent.ItemType<EternianTinkererHammer>(), Condition.DownedOldOnesArmyT1);
                shop.Add(eternianHammer);
            }
        }

    }
}
