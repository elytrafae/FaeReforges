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

    }
}
