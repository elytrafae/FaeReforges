using FaeLibrary.API.Cooldown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Cooldowns {
    internal class HellstoneHammerCooldown : FaeSimpleCooldown {
        public override int CooldownTicks => 180;
        public override int Charges => 1;
    }
}
