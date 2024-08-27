using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.HammerTypes {
    public class StoneHammer : AbstractHammerType {
        public override void SetDefaults() {
            this.negativeReforgeChance = 70;
            this.reforgeCost = 150;
        }

    }
}
