using FaeReforges.Systems.ReforgeHammers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using static Terraria.UI.ItemSlot;

namespace FaeReforges.Systems.UI.UIElements {
    internal class GoblinTinkererHammerUIItemSlotOld : UIItemSlot {
        public GoblinTinkererHammerUIItemSlotOld() : base(ReforgeHammerSavePlayer.GetReforgeHammerStorageOfMyPlayer(), 0, Context.BankItem) {
        }

        
    }
}
