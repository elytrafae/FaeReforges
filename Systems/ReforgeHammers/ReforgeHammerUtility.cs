using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace FaeReforges.Systems.ReforgeHammers {
    public static class ReforgeHammerUtility {

        public static int GetHammerItemType(Item item) {
            if (item == null || item.type == ItemID.None) {
                return ItemID.None;
            }
            return item.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammerItemTypeOrNone();
        }

        public static bool HasAnySummonHammer(Player player, int hammerType) {
            foreach (Projectile proj in Main.ActiveProjectiles) {
                if (proj.owner == player.whoAmI && (proj.sentry || proj.minion) && proj.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>().GetHammerItemTypeOrNone() == hammerType) {
                    return true;
                }
            }
            return false;
        }

    }
}
