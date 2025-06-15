using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaeReforges.Systems.ReforgeHammers;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace FaeReforges.Content.Rarities {
    internal class PhantomRarity : ModRarity {
        public override Color RarityColor => ReforgeHammerSavePlayer.GetPhantomColor();

        public override int GetPrefixedRarity(int offset, float valueMult) {
            return Type; // Just return to itself, I guess. Won't be used for any items that can be reforged, I hope.
        }

    }
}
