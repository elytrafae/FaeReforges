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
    internal class GoblinTinkererHammerUIItemSlot : UIItemSlot {
        public GoblinTinkererHammerUIItemSlot() : base(ReforgeHammerSaveSystem.reforgeHammerStorage, 0, Context.BankItem) {


        }

        
    }
}
