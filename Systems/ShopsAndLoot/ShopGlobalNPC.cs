using FaeReforges.Content.Items.TinkererHammers.Tier2;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Content.Items.TinkererHammers.Tier4;
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
            switch (shop.NpcType) {
                case NPCID.DD2Bartender:
                    Add<EternianTinkererHammer>(shop, Condition.DownedOldOnesArmyT1);

                    Add<SquireTinkererHammer>(shop, Condition.DownedOldOnesArmyT2);
                    Add<HuntressTinkererHammer>(shop, Condition.DownedOldOnesArmyT2);
                    Add<ApprenticeTinkererHammer>(shop, Condition.DownedOldOnesArmyT2);
                    Add<MonkTinkererHammer>(shop, Condition.DownedOldOnesArmyT2);
                    break;

                case NPCID.WitchDoctor:
                    Add<TikiTinkererHammer>(shop, Condition.DownedGolem);
                    break;

                case NPCID.BestiaryGirl:
                    Add<RiderTinkererHammer>(shop, Condition.DownedGolem, Condition.BestiaryFilledPercent(60));
                    break;

                case NPCID.PartyGirl:
                    Add<PartyTinkererHammer>(shop, Condition.DownedGolem);
                    break;

            }
        }

        private static void Add<T>(NPCShop shop, params Condition[] condition) where T : ModItem { 
            shop.Add(ModContent.ItemType<T>(), condition);
        }

    }
}
