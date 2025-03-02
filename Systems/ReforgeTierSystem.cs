using FaeReforges.Content.Reforges;
using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    internal class ReforgeTierSystem : ModSystem {

        private static readonly int[] prefixTiers = PrefixID.Sets.Factory.CreateIntSet(0);
        private const int FIRST_ACCESSORY_PREFIX = 62;
        private const int LAST_ACCESSORY_PREFIX = 80;
        private static readonly Dictionary<int, List<int>> PrefixTierCache = new(); 

        public override void Load() {
            Item dummyItem = new Item(ItemID.CelestialShell);
            int initPrice = dummyItem.value;
            for (int i = FIRST_ACCESSORY_PREFIX; i <= LAST_ACCESSORY_PREFIX; i++) {
                dummyItem.value = initPrice;
                dummyItem.Prefix(i);
                int newPrice = dummyItem.value;
                double priceDiff = ((double)newPrice) / initPrice;
                prefixTiers[i] = GetTierFromPriceDiff(priceDiff);
            }
        }

        public static int GetPrefixTier(int pre) {
            return prefixTiers[pre];
        }

        public static void SetPrefixTier(int pre, int tier) {
            prefixTiers[pre] = tier;
        }

        // Only use on vanilla items!
        private static int GetTierFromPriceDiff(double priceDiff) {
            if (priceDiff < 1.15) {
                return 1;
            }
            if (priceDiff < 1.26) {
                return 2;
            }
            if (priceDiff < 1.39) {
                return 3;
            }
            return 4;
        }

        public static float GetValueMult(int tier) {
            return 1f + tier * 0.05f;
        }

        public static float GetPriceMultForType(int type) { 
            return GetValueMult(GetPrefixTier(type));
        }

        public static IEnumerable<int> GetAllReforgesOfTier(int tier) {
            List<int> list;
            if (PrefixTierCache.TryGetValue(tier, out list)) {
                return list;
            }
            list = new List<int>();
            for (int i = 0; i < prefixTiers.Length; i++) {
                if (prefixTiers[i] == tier) {
                    list.Add(i);
                }
            }
            PrefixTierCache.Add(tier, list);
            return list;
        }

    }
}
