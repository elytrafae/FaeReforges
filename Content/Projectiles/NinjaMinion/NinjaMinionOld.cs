using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using FaeReforges.Content.Buffs;

namespace FaeReforges.Content.Projectiles.NinjaMinion {
    internal class NinjaMinionOld : ModProjectile {

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.MinionSacrificable[Type] = false;
            Main.projPet[Type] = true;
        }

        private bool flying = false;
        private bool jumping = false;
        private bool isFarAway = false;

        public override void SetDefaults() {
            Projectile.width = 24;
            Projectile.height = 44;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            if (Projectile.TryGetOwner(out Player owner)) {
                fallThrough = owner.Center.Y > Projectile.Center.Y + 8;
            }
            return !isFarAway;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (oldVelocity.Y > 0 && oldVelocity.Y >= 0) { // If we just landed...
                jumping = false;
                flying = false;
            }
            return false;
        }

        public override bool? CanCutTiles() {
            return false;
        }

        public override bool? CanDamage() {
            return false;
        }

        public override bool MinionContactDamage() {
            return false;
        }

        public override void AI() {
            if (!Projectile.TryGetOwner(out Player owner)) {
                Projectile.Kill();
                return;
            }
            if (!owner.HasBuff<NinjaMinionBuff>()) {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            Movement(owner);


        }

        private float JumpVelocity => 4f;
        private float VerticalFlightAcceleration => isFarAway ? 0.1f : 0.06f;
        private float VerticalFlightMaxSpeed => isFarAway ? 20f : 10f;
        private float HorizontalAcceleration => isFarAway ? 0.1f : 0.06f;
        private float HorizontalMaxSpeed => isFarAway ? 16f : 8f;
        private int BreakDistance = 32;

        private void Movement(Player owner) {
            Vector2 targetPos = owner.position + new Vector2(owner.width / 2 + owner.direction * -80, owner.height);
            Vector2 currentPos = Projectile.position + new Vector2(Projectile.width / 2, Projectile.height);
            Vector2 distance = targetPos - currentPos;

            isFarAway = distance.Length() > 800f;

            if (isFarAway) {
                jumping = false;
                flying = true;
            }

            if (distance.Y < -16) { // We need to ascend
                if (!jumping && !flying) { // We are on the ground. Try jumping to reach the new height
                    Jump();
                } else if (jumping && Projectile.velocity.Y >= 0) { // We are already jumping, but the jump isn't enough. Start flying
                    ProcessFlyingUpwards();
                } else { // We are most definitely already flying
                    ProcessFlyingUpwards();
                }
            } else if (distance.Y > 16) { // We need to descend
                if (flying) {
                    ProcessFlyingDownwards();
                    if (!isFarAway && AreThereSolidTilesBelow(currentPos)) {
                        flying = false;
                    }
                }
            }



            if (distance.Y > -BreakDistance && distance.Y < BreakDistance) {
                Projectile.velocity.Y *= 0.9f;
            }

            if (!flying) {
                Projectile.velocity.Y += 0.08f; // Gravity
            }

            Projectile.velocity.X += (distance.X > 0 ? 1 : -1) * HorizontalAcceleration;
            if (Projectile.velocity.X > HorizontalMaxSpeed) {
                Projectile.velocity.X = HorizontalMaxSpeed;
            }
            if (Projectile.velocity.X < -HorizontalMaxSpeed) {
                Projectile.velocity.X = -HorizontalMaxSpeed;
            }

            if (distance.X > -BreakDistance && distance.X < BreakDistance) {
                Projectile.velocity.X *= 0.4f; // Attempt to decelerate when close to horizontal target
            }


        }

        private void ProcessFlyingUpwards() {
            Projectile.velocity.Y -= VerticalFlightAcceleration;
            if (Projectile.velocity.Y < -VerticalFlightMaxSpeed) {
                Projectile.velocity.Y = -VerticalFlightMaxSpeed;
            }
            jumping = false;
            flying = true;
        }

        private void ProcessFlyingDownwards() {
            Projectile.velocity.Y += VerticalFlightAcceleration;
            if (Projectile.velocity.Y > VerticalFlightMaxSpeed) {
                Projectile.velocity.Y = VerticalFlightMaxSpeed;
            }
            jumping = false;
            flying = true;
        }

        private void Jump() {
            Projectile.velocity.Y = -JumpVelocity;
            jumping = true;
            flying = false;
        }

        private bool AreThereSolidTilesBelow(Vector2 currentPos) {
            Point tileCoords = new((int)(currentPos.X / 16), (int)(currentPos.Y / 16));
            if (Main.tile[tileCoords].TopSlope) {
                return true;
            }
            tileCoords.Y -= 1;
            if (Main.tile[tileCoords].TopSlope) {
                return true;
            }
            tileCoords.X -= 1;
            if (Main.tile[tileCoords].TopSlope) {
                return true;
            }
            tileCoords.X += 2;
            if (Main.tile[tileCoords].TopSlope) {
                return true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor) {
            lightColor = flying ? Color.ForestGreen : Color.SkyBlue;
            return true;
        }

    }
}
