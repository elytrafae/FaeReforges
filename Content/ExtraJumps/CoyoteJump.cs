using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

// TODO: Move this to another mod . . . ?
namespace FaeReforges.Content.ExtraJumps {
    internal class CoyoteJump : ExtraJump {
        public override Position GetDefaultPosition() {
            return new ExtraJump.After(Flipper);
        }

        public override float GetDurationMultiplier(Player player) {
            return 1f;
        }

        public override void OnStarted(Player player, ref bool playSound) {
            SoundEngine.PlaySound(SoundID.AbigailCry);
        }
    }
}
