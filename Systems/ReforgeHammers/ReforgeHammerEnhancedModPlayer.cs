using FaeReforges.Content.Items;
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

        public Dictionary<int, int> AccessoryReforgeCounts = new Dictionary<int, int>();

        public override void ResetEffects() {
            AccessoryReforgeCounts.Clear();
        }

    }
}
