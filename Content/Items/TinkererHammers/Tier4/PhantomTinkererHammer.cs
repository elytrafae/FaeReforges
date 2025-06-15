using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Rarities;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    internal class PhantomTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ModContent.RarityType<PhantomRarity>();
        public override ItemCondition ReforgeableCondition => ItemCondition.Any;
        public override int HammerTier => 4;
        public override bool PhantomHammer => true;
    }
}
