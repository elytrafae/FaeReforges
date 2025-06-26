using FaeReforges.Content.Reforges;
using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {
    public class ReforgeTierSystem : ModSystem {

        private const int FIRST_ACCESSORY_PREFIX = 62;
        private const int LAST_ACCESSORY_PREFIX = 80;
        private static readonly Dictionary<int, List<int>> PrefixTierCache = new();

        private static bool DEBUG_SET_STATIC_DEFAULTS_OVER = false;

        public override void SetStaticDefaults() {
            Item dummyItem = new Item(ItemID.CelestialShell);
            int initPrice = dummyItem.value;
            for (int i = FIRST_ACCESSORY_PREFIX; i <= LAST_ACCESSORY_PREFIX; i++) {
                dummyItem.value = initPrice;
                dummyItem.Prefix(i);
                int newPrice = dummyItem.value;
                double priceDiff = ((double)newPrice) / initPrice;
                CustomIDSets.PrefixTiers[i] = GetTierFromPriceDiff(priceDiff);
            }

            for (int i=0; i < DynamicReforgeLoader.vanillaOverrides.Length; i++) {
                VanillaReforgeOverrideData data = DynamicReforgeLoader.vanillaOverrides[i];
                if (data != null) {
                    CustomIDSets.PrefixTiers[i] = data.tier;
                }
            }

            DEBUG_SET_STATIC_DEFAULTS_OVER = true;
        }

        public override void Load() {
            
        }

        public override void Unload() {
            DEBUG_SET_STATIC_DEFAULTS_OVER = false;
        }

        public static int GetPrefixTier(int pre) {
            return CustomIDSets.PrefixTiers[pre];
        }

        public static void SetPrefixTier(int pre, int tier) {
            CustomIDSets.PrefixTiers[pre] = tier;
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
            for (int pre = 1; pre < CustomIDSets.PrefixTiers.Length; pre++) {
                if (CustomIDSets.PrefixTiers[pre] == tier) {
                    list.Add(pre);
                }
            }
            PrefixTierCache.Add(tier, list);
            return list;
        }

        // ReforgeCacheListToReadableNames(GetAllReforgesOfTier(4))
        private static List<string> ReforgeCacheListToReadableNames(IEnumerable<int> reforges) {
            List<string> names = new();
            foreach (int pre in reforges) {
                names.Add(Lang.prefix[pre].Value);
            }
            return names;
        }

    }
}
