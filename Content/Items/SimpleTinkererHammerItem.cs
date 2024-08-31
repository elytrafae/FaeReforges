using FaeReforges.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items {
    public abstract class SimpleTinkererHammerItem : ModItem {

        public abstract int Rarity { get; }
        public abstract int Value { get; }

        public sealed override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.rare = Rarity;
            Item.maxStack = 1;
            Item.value = Value;
        }

    }
}
