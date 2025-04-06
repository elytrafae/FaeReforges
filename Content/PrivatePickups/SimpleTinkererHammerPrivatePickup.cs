using FaeReforges.Content.Buffs;
using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Systems;
using FaeReforges.Systems.PrivatePickups;
using FaeReforges.Systems.ReforgeHammers;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using FaeReforges.Content.Items;

namespace FaeReforges.Content.PrivatePickups {
    public class SimpleTinkererHammerPrivatePickup : PrivatePickup {

        public int timer = 0;

        public int dustType = DustID.Adamantite;
        public Color dustColor = default;
        public SoundStyle killSound = new();
        public int dustFrequency = 6;
        public int lifetime = 400;
        public int hammerType = ItemID.None;

        public virtual void SimpleSetDefaults() { }

        public sealed override void SetDefaults() {
            width = 64;
            height = 64;
            timer = 0;
            SimpleSetDefaults();
        }

        public virtual void SimpleUpdate() { }
        public virtual bool SimpleOnCollide(Player player) { return true; }
        public virtual bool SimpleOnCollide(Projectile projectile) { return true; }

        public sealed override void Update() {
            timer++;
            SimpleUpdate();
            if (timer % dustFrequency == 0) {
                Dust.NewDust(position, width, height, dustType, 0, 0, 0, dustColor);
            }
            if (timer > lifetime) {
                Kill();
            }
            if (ReforgeHammerUtility.GetHammerItemType(Main.LocalPlayer.HeldItem) != hammerType) {
                Kill();
            }
        }

        public sealed override bool OnCollidePlayer(Player player) {
            SoundEngine.PlaySound(SoundID.Shatter.WithVolumeScale(0.7f));
            return SimpleOnCollide(player);
        }

        public sealed override bool OnCollideProjectile(Projectile projectile) {
            SoundEngine.PlaySound(SoundID.Shatter.WithVolumeScale(0.7f));
            return SimpleOnCollide(projectile);
        }

        public sealed override void PreKill() {
            for (int i = 0; i < 20; i++) {
                Dust.NewDust(position, width, height, dustType, 0, 0, 0, dustColor);
            }
            SoundEngine.PlaySound(killSound);
        }

    }
}
