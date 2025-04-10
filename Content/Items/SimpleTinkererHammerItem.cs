using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items {
    public abstract class SimpleTinkererHammerItem : AbstractTinkererHammer {

        public abstract int Rarity { get; }
        public abstract int Value { get; }
        public virtual int? CustomPrice => null;
        public virtual int CustomCurrency => CustomCurrencyID.None;
        public override abstract ItemCondition ReforgeableCondition { get; }
        public override string LocalizationCategory => base.LocalizationCategory + ".ReforgeHammers.Tier" + HammerTier;

        public sealed override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.rare = Rarity;
            Item.maxStack = 1;
            Item.value = Value;
            Item.shopCustomPrice = CustomPrice;
            Item.shopSpecialCurrency = CustomCurrency;
        }

    }
}
