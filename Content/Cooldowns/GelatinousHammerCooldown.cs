using FaeLibrary.API.Cooldown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Cooldowns {
    internal class GelatinousHammerCooldown : FaeSimultaniousChargeCooldown {
        public override int CooldownTicks => 10*60;
        public override int Charges => 5;
    }
}
