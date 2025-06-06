using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FaeReforges.Content.Projectiles.NinjaMinion {
    public class NinjaMinionCritShot : NinjaMinionShot {

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
            modifiers.SetCrit();
        }

    }
}
