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
using Terraria.Audio;
using System.IO;
using Terraria.ModLoader.IO;

namespace FaeReforges.Content.Projectiles.NinjaMinion {
    internal class NinjaMinion : ModProjectile {

        const int SALVO_COUNT = 4;
        const int SALVO_INTERVAL = 3;
        const int SALVO_COOLDOWN = 50;

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.MinionSacrificable[Type] = false;
            Main.projPet[Type] = true;
        }

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

        private bool IsStuckHorizontally = false;
        private int OnGroundFrames = 0;
        private int JumpCooldown = 0;
        private int StuckJumps = 0;
        private int StuckJumpsTimer = 0;
        private int SalvoTimer = 0;
        private Vector2? AttackTargetPos = null;

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(IsStuckHorizontally);
            writer.Write(OnGroundFrames);
            writer.Write(JumpCooldown);
            writer.Write(StuckJumps);
            writer.Write(StuckJumpsTimer);
            writer.Write(SalvoTimer);
            writer.Write(AttackTargetPos.HasValue);
            if (AttackTargetPos.HasValue) {
                writer.WriteVector2(AttackTargetPos.Value);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            IsStuckHorizontally = reader.ReadBoolean();
            OnGroundFrames = reader.ReadInt32();
            JumpCooldown = reader.ReadInt32();
            StuckJumps = reader.ReadInt32();
            StuckJumpsTimer = reader.ReadInt32();
            SalvoTimer = reader.ReadInt32();
            if (reader.ReadBoolean()) {
                AttackTargetPos = reader.ReadVector2();
            } else {
                AttackTargetPos = null;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            if (Projectile.TryGetOwner(out Player owner)) {
                fallThrough = owner.Center.Y > Projectile.Center.Y + 8;
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (oldVelocity.X > 0 && Projectile.velocity.Y <= 0) {
                OnGroundFrames = 2;
            }
            if (oldVelocity.X != 0 && Projectile.velocity.X == 0) {
                IsStuckHorizontally = true;
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
            if (owner.dead) {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            Movement(owner);
            Attack(owner);
            // TODO: Animate the Ninja
        }

        private float JumpVelocity => 5f;
        private int BreakDistance = 32;
        private float HorizontalAcceleration => 0.06f;
        private float HorizontalMaxSpeed => 8f;

        private void Movement(Player owner) {
            Vector2 targetPos = owner.position + new Vector2(owner.width/2 + owner.direction * -80, owner.height);
            Vector2 currentPos = Projectile.position + new Vector2(Projectile.width / 2, Projectile.height);
            Vector2 distance = targetPos - currentPos;

            if (distance.Length() > 1200f && owner.whoAmI == Main.myPlayer) {
                Teleport(targetPos);
            }

            if (distance.Y < -32) { // We need to ascend
                if (Projectile.velocity.Y >= 0) {
                    Jump(currentPos);
                }
            } else {
                if (IsStuckHorizontally) {
                    if (Jump(currentPos)) {
                        IsStuckHorizontally = false;
                    }
                }
            }

            Projectile.velocity.Y += 0.08f; // Gravity
            OnGroundFrames--;
            if (Projectile.velocity.Y > 32) {
                Projectile.velocity.Y = 32;
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

            JumpCooldown--;
            StuckJumpsTimer++;
            if (StuckJumpsTimer >= 40) {
                StuckJumps--;
                StuckJumpsTimer = 0;
            }
            if (StuckJumps > 3) {
                Teleport(targetPos);
                StuckJumps = 0;
            }

            if (Projectile.ai[0] == 1) { // We teleported, play appropriate effects!
                Vector2 oldPos = new(Projectile.ai[1], Projectile.ai[2]);
                SoundEngine.PlaySound(SoundID.Item6, Projectile.Center);
                SpawnTeleportEffectsAt(Projectile.position);
                SpawnTeleportEffectsAt(oldPos);
                Projectile.ai[0] = 0;
            }
        }

        private void SpawnTeleportEffectsAt(Vector2 position) {
            int goreCount = Main.rand.Next(8, 16);
            int dustCount = Main.rand.Next(7, 20);
            for (int i = 0; i < goreCount; i++) {
                Gore.NewGore(Projectile.GetSource_FromAI(), position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)), new Vector2(0, 0), 61 + Main.rand.Next(3), Main.rand.NextFloat() + 0.5f);
            }
            for (int i = 0; i < dustCount; i++) {
                Dust.NewDust(position, Projectile.width, Projectile.height, DustID.Cloud, 0, 0, 0, Color.Brown);
            }
        }

        private bool Jump(Vector2 currentPos) {
            if (JumpCooldown > 0) {
                return false;
            }
            StuckJumps++;
            Projectile.velocity.Y = -JumpVelocity;
            JumpCooldown = 20;
            if (OnGroundFrames <= 0) {
                SoundEngine.PlaySound(SoundID.DoubleJump, Projectile.Center);
                int goreCount = Main.rand.Next(3, 5);
                int dustCount = Main.rand.Next(5, 12);
                for (int i = 0; i < goreCount; i++) {
                    Gore.NewGore(Projectile.GetSource_FromAI(), currentPos + new Vector2(Main.rand.Next(10) - 20, 0), new Vector2(0, 0.03f + Main.rand.NextFloat() * 0.4f), 11 + Main.rand.Next(3), Main.rand.NextFloat() * 0.6f + 0.4f);
                }
                for (int i = 0; i < dustCount; i++) {
                    Dust.NewDust(currentPos - new Vector2(10, 3), 20, 7, DustID.Cloud);
                }
            }
            return true;
        }

        private void Teleport(Vector2 targetPos) {
            Projectile.ai[1] = Projectile.position.X;
            Projectile.ai[2] = Projectile.position.Y;
            Projectile.position = targetPos;
            Projectile.velocity = Vector2.Zero;
            Projectile.netUpdate = true;
            Projectile.ai[0] = 1;
        }

        private void Attack(Player owner) {
            SalvoTimer++;

            // First frame before salvo, ran only on current client and then synced!
            // Purpose is to find a target
            if (SalvoTimer == SALVO_COOLDOWN-1 && owner.whoAmI == Main.myPlayer) {
                FindTarget(owner);
                if (!AttackTargetPos.HasValue) {
                    SalvoTimer -= 8; // Halt the salvo and keep looking.
                }
                Projectile.netUpdate = true;
            }

            if (SalvoTimer >= SALVO_COOLDOWN) {
                int timerRest = SalvoTimer - SALVO_COOLDOWN;
                if (timerRest % SALVO_INTERVAL == 0) {
                    int currentProjectile = timerRest / SALVO_INTERVAL + 1;
                    bool lastShurikenInSalvo = currentProjectile >= SALVO_COUNT;

                    // Projectile spawning, only for owner!
                    if (owner.whoAmI == Main.myPlayer) {
                        int projectileType = lastShurikenInSalvo ? ModContent.ProjectileType<NinjaMinionCritShot>() : ModContent.ProjectileType<NinjaMinionShot>();
                        Vector2 velocity = AttackTargetPos.Value - Projectile.Center;
                        velocity.Normalize();
                        velocity *= 10f;

                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, projectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }


                    // TODO: Animate Ninja attacking for everyone

                    // Reset salvo variables
                    if (lastShurikenInSalvo) {
                        SalvoTimer = 0;
                    }
                }
            }
        }

        private void FindTarget(Player owner) {
            if (owner.HasMinionAttackTargetNPC) {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f) {
                    AttackTargetPos = npc.Center;
                    return;
                }
            }

            float closestTargetDistance = 700f;
            Vector2 closestTargetPos = Projectile.position;
            bool foundTarget = false;
            // This code is required either way, used for finding a target
            foreach (var npc in Main.ActiveNPCs) {
                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    bool closest = Vector2.Distance(Projectile.Center, closestTargetPos) > between;
                    bool inRange = between < closestTargetDistance;
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

                    if (((closest && inRange) || !foundTarget) && lineOfSight) {
                        closestTargetDistance = between;
                        closestTargetPos = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            if (foundTarget) {
                AttackTargetPos = closestTargetPos;
            } else {
                AttackTargetPos = null;
            }
        }

    }
}
