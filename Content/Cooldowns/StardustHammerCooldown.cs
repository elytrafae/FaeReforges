using FaeLibrary.API.Cooldown;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FaeReforges.Content.Cooldowns {
    public class StardustHammerCooldown : FaeSimpleCooldown {
        public override int CooldownTicks => 15 * 60;
        public override int Charges => 2;
        public override int GetCooldownTickRate() {
            return MyReforgeHammerPlayer2.Get(Main.LocalPlayer).stardustDuration > 0 ? 0 : 1;
        }
    }
}
