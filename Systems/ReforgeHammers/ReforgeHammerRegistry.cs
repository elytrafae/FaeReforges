using FaeReforges.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerRegistry : ModSystem {

        private static Dictionary<int, ReforgeHammerType> hammerMap = new();

        public override void Load() {
            // Add loading for hammers!
        }

        public override void Unload() {
            hammerMap.Clear();
        }

        public static ReforgeHammerType GetHammerTypeForItemType(int type) {
            if (!hammerMap.ContainsKey(type)) {
                return null;
            }
            return hammerMap[type];
        }

        public static void RegisterHammerType(Item item, ReforgeHammerType hammer) {
            hammer.SetupLocalization(item);
            hammerMap[item.type] = hammer;
        }

    }
}
