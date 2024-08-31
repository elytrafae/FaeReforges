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

        private static int[] vanillaAccessoryPrefixTiers = new int[PrefixID.Count];

        private const int FIRST_ACCESSORY_PREFIX = 62;
        private const int LAST_ACCESSORY_PREFIX = 80;

        public override void Load() {
            Item dummyItem = new Item(ItemID.CelestialShell);
            int initPrice = dummyItem.value;
            for (int i = FIRST_ACCESSORY_PREFIX; i <= LAST_ACCESSORY_PREFIX; i++) {
                dummyItem.value = initPrice;
                dummyItem.Prefix(i);
                int newPrice = dummyItem.value;
                double priceDiff = ((double)newPrice) / initPrice;
                vanillaAccessoryPrefixTiers[i] = GetTierFromPriceDiff(priceDiff);
            }
        }

        public static bool IsPrefixPositive(int pre) {
            ModPrefix modPrefix = PrefixLoader.GetPrefix(pre);
            if (modPrefix == null) {
                VanillaReforgeOverrideData data = DynamicReforgeLoader.vanillaOverrides[pre];
                if (data == null) {
                    return false; // IDK what to do here. This shouldn't even be called in this case!
                }
                return data.positive;
            }
            float priceMult = 1f;
            modPrefix.ModifyValue(ref priceMult);
            return priceMult >= 1f;
        }

        public static int GetAccessoryPrefixTier(int pre) {
            ModPrefix modPrefix = PrefixLoader.GetPrefix(pre);
            if (modPrefix == null) {
                return vanillaAccessoryPrefixTiers[pre];
            }
            float priceMult = 1f;
            modPrefix.ModifyValue(ref priceMult);
            return GetTierFromPriceMult(priceMult);
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


        // Only use on modded items!
        private static int GetTierFromPriceMult(double priceMult) {
            return (int)Math.Round((priceMult - 1) / 0.05);
        }

        public static bool IsPrefixForAccessories(int pre) {
            if (pre >= FIRST_ACCESSORY_PREFIX && pre <= LAST_ACCESSORY_PREFIX) {
                return true;
            }
            ModPrefix modPre = PrefixLoader.GetPrefix(pre);
            if (modPre == null) {
                return false;
            }
            return modPre.Category == PrefixCategory.Accessory;
        }

    }
}
