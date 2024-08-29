using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Content {
    public class SystemToLoadAllHammerTypes : ModSystem {

        public override void OnModLoad() {
            ReforgeHammerType stoneType = new ReforgeHammerType();
            stoneType.negativeReforgeChance = 75;
            stoneType.reforgeCost = 150;
            ReforgeHammerRegistry.RegisterHammerType(ModContent.GetInstance<StoneTinkererHammer>().Item, stoneType);

            ReforgeHammerType mythrilType = new ReforgeHammerType();
            mythrilType.negativeReforgeChance = 45;
            mythrilType.reforgeCost = 115;
            ReforgeHammerRegistry.RegisterHammerType(ModContent.GetInstance<MythrilTinkererHammer>().Item, mythrilType);
        }

    }
}
