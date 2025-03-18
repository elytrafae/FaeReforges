using FaeLibrary.API.Cooldown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaeReforges.Content.Cooldowns {
    internal class HoneycombHammerCooldown : FaeSimpleCooldown {
        public override int CooldownTicks => 5*60;
        public override int Charges => 1;
    }
}

// TODO: Test accessory for summon speed
// TODO: See what's wrong with longswords!
// TODO: Remake the Summon Weapon VS Whip distinction