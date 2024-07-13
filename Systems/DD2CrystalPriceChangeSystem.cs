using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    public class DD2CrystalPriceChangeSystem : ModSystem {

        public override void Load() {
            On_Player.GetRequiredDD2CrystalsToUse += On_Player_GetRequiredDD2CrystalsToUse;
        }

        private int On_Player_GetRequiredDD2CrystalsToUse(On_Player.orig_GetRequiredDD2CrystalsToUse orig, Player self, Item item) {
            int origCost = orig.Invoke(self, item);
            if (item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) { 
                return (int)(origCost * globItem.minionOccupancyMult);
            }
            return origCost;
        }
    }
}
