using FaeLibrary.API.UI;
using FaeReforges.Content.Items;
using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.UI.ItemSlot;

namespace FaeReforges.Systems.UI.UIElements {
    public class GoblinTinkererHammerUIItemSlot : FaeCustomUIItemSlot {

        public GoblinTinkererHammerUIItemSlot() : base(ReforgeHammerSavePlayer.GetReforgeHammerStorageOfMyPlayer(), 0, 0.75f, Context.BankItem) {
        }

        public override bool CanInsertItem(Item item) {
            return item.ModItem is AbstractTinkererHammer;
        }

    }
}
