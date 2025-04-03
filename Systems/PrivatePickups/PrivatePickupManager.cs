using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace FaeReforges.Systems.PrivatePickups {
    public class PrivatePickupManager : ModSystem {

        public const int MAX_COUNT = 20;
        private static PrivatePickup[] PrivatePickups = new PrivatePickup[MAX_COUNT];

        public override void Load() {
            for (int i = 0; i < PrivatePickups.Length; i++) {
                PrivatePickups[i] = new PrivatePickup(); // Makes sure nothing is ever null
            }
            On_Main.DrawProjectiles += On_Main_DrawProjectiles;
        }

        public static T? Spawn<T>(float x, float y) where T : PrivatePickup, new() {
            int slot = FindEmptyPickupSlot();
            if (slot < 0) {
                return null; // Could not spawn because slots are full
            }
            T pickup = new T();
            pickup.active = true;
            pickup.SetDefaults();
            pickup.Center = new Vector2(x, y);
            pickup.whoAmI = slot;
            PrivatePickups[slot] = pickup;
            return pickup;
        }

        private static int FindEmptyPickupSlot() {
            for (int i = 0; i < MAX_COUNT; i++) {
                if (PrivatePickups[i] == null || !PrivatePickups[i].active) {
                    return i;
                }
            }
            return -1;
        }

        public static PrivatePickup GetByWhoAmI(int whoAmI) {
            return PrivatePickups[whoAmI];
        }

        public static ActiveEntityIterator<PrivatePickup> ActivePrivatePickups => new(PrivatePickups.AsSpan(0, MAX_COUNT));

        public static int CountByType<T>() where T : PrivatePickup {
            int count = 0;
            foreach (PrivatePickup pickup in ActivePrivatePickups) {
                count += ((pickup is T) ? 1 : 0);
            }
            return count;
        }

        // Arbitrarily chosen update phase
        public override void PostUpdateProjectiles() {
            foreach (PrivatePickup pickup in ActivePrivatePickups) {
                pickup.Update();
                Rectangle pickupRect = pickup.getRect();
                if (pickup.reactToPlayer) {
                    if (Main.LocalPlayer.getRect().Intersects(pickupRect)) {
                        if (pickup.OnCollidePlayer(Main.LocalPlayer)) {
                            pickup.Kill();
                        }
                    }
                }
                if (pickup.reactToProjectiles) {
                    int i = 0;
                    while (pickup.active && i < Main.projectile.Length) {
                        Projectile proj = Main.projectile[i];
                        if (proj != null && proj.active && proj.owner == Main.myPlayer) {
                            if (proj.getRect().Intersects(pickupRect)) {
                                if (pickup.OnCollideProjectile(proj)) {
                                    pickup.Kill();
                                }
                            }
                        }
                        i++;
                    }
                }
            }
        }

        private void On_Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self) {
            PlayerInput.SetZoom_MouseInWorld();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.CurrentDrawnEntity = null;
            Main.CurrentDrawnEntityShader = 0;
            foreach (PrivatePickup pickup in ActivePrivatePickups) {
                pickup.Draw(Main.spriteBatch);
            }
            Main.CurrentDrawnEntity = null;
            Main.CurrentDrawnEntityShader = 0;
            Main.spriteBatch.End();

            orig(self);
        }

    }
}
