using FaeReforges.Content.PrivatePickups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammerContent {
    public class MyReforgeHammerProjectile : GlobalProjectile {

        public override bool InstancePerEntity => true;

        private List<TargetPickup> targetsAlreadyHit = new();
        private int targetCountAlreadyHit = 0;

        public bool RegisterHitTarget(TargetPickup target) {
            if (targetsAlreadyHit.Contains(target)) {
                return false;
            }
            targetsAlreadyHit.Add(target);
            targetCountAlreadyHit++;
            return true;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            modifiers.SourceDamage *= (int)Math.Pow(2, targetCountAlreadyHit);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            modifiers.SourceDamage *= (int)Math.Pow(2, targetCountAlreadyHit);
        }

        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) {
            binaryWriter.Write(targetCountAlreadyHit);
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) {
            targetCountAlreadyHit = binaryReader.ReadInt32();
        }

        

    }
}
