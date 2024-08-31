using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerEnhancedModPlayer : ModPlayer {

        public override void PostUpdateEquips() {
            if (Player.HeldItem != null && Player.HeldItem.type != ItemID.None) {
                Player.HeldItem.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer()?.onUpdateWeaponHeld(Player.HeldItem, Player);
            }
            
        }

    }
}
