using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Systems;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FaeReforges.Systems.ReforgeHammerContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using FullSerializer.Internal;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.PrivatePickups {
    public class TargetPickup : SimpleTinkererHammerPrivatePickup {

        bool onCountdown = false;

        public override void SimpleSetDefaults() {
            texture = MiscSpritesSystem.TargetPickup;
            reactToProjectiles = true;
            dustType = DustID.Adamantite;
            killSound = SoundID.NPCHit15.WithPitchOffset(-2f).WithVolumeScale(0.7f);
            hammerType = ModContent.ItemType<HuntressTinkererHammer>();
            enableShatterSoundOnTouch = false;
        }

        private void StartFinalCountdown() {
            if (onCountdown) {
                return;
            }
            onCountdown = true;
            timer = 61;
            color = new Color(0, 194, 0);
        }

        public override bool SimpleOnCollide(Projectile projectile) {
            if (ReforgeHammerUtility.GetHammerItemType(projectile) != hammerType) {
                return false;
            }
            if (projectile.GetGlobalProjectile<MyReforgeHammerProjectile>().RegisterHitTarget(this)) {
                SoundEngine.PlaySound(SoundID.NPCHit17.WithVolumeScale(0.6f));
            }
            StartFinalCountdown();
            return false;
        }

    }
}
