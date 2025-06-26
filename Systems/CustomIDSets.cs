using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems {

    [ReinitializeDuringResizeArrays]
    public class CustomIDSets {

        public static int[] PrefixTiers = PrefixID.Sets.Factory.CreateNamedSet("PrefixTiers")
            .Description("The tier of the reforge in question. See the mod's functionality for details. While any prefix can be obtained with the corresponding tier hammer, -1 should be the default for 'no'. Developers should avoid making hammers with negative tiers!")
            .RegisterIntSet(-1);

        public static bool[] ShouldReceiveAccessoryReforgeHammerBenefits = ItemID.Sets.Factory.CreateNamedSet("ShouldReceiveAccessoryReforgeHammerBenefits")
            .Description("Should this item receive accessory effects from hammers? true by default. Set to false to make the accessory reforgable still while making it unable to get accessory hammer effects")
            .RegisterBoolSet(true);

        public static bool[] ShouldReceiveWeaponReforgeHammerBenefits = ItemID.Sets.Factory.CreateNamedSet("ShouldReceiveWeaponReforgeHammerBenefits")
            .Description("Should this item receive weapon effects from hammers? true by default. Set to false to make the weapon reforgable still while making it unable to get weapon hammer effects")
            .RegisterBoolSet(true);

        public static bool[] NoSummonerReforge = ItemID.Sets.Factory.CreateNamedSet("NoSummonerReforge")
            .Description("Set to true to make the item unable to get summoner reforges regardless of config options, resulting in the item receiving mage reforges by default. Useful for summoner weapons that are glorified mage weapons")
            .RegisterBoolSet(false);

    }
}
