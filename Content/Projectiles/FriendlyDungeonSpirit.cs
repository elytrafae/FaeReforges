using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Projectiles {
    public class FriendlyDungeonSpirit : ModProjectile {

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 3;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            Main.projPet[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.penetrate = 3;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 10 * 60;
            Projectile.tileCollide = false;
            Projectile.ArmorPenetration = 10;
            Projectile.OriginalArmorPenetration = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI() {
            if (!Projectile.TryGetOwner(out Player owner)) {
                return;
            }

            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter);
            Visuals();
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles() {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage() {
            return true;
        }

        // Code taken from ExampleMod
        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC) {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f) {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget) {
                // This code is required either way, used for finding a target
                foreach (var npc in Main.ActiveNPCs) {
                    if (npc.CanBeChasedBy()) {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            Projectile.friendly = foundTarget;
        }


        // Code taken from ExampleMod, with the small change that the Dungeon Spirit will slow down and "freeze" if there is no target, similar to the Snowflake minion
        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter) {
            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20f;

            if (foundTarget) {
                // Minion has a target: attack (here, fly towards the enemy)
                if (distanceFromTarget > 40f) {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            } else {
                Projectile.velocity /= 1.4f;
            }
        }

        private void Visuals() {
            // Animate sprite
            if (++Projectile.frameCounter >= 5) {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            // Some visuals here
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.78f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            OnHitEffects();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            OnHitEffects();
        }

        private void OnHitEffects() {
            for (int i = 0; i < 5; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy);
            }
            SoundEngine.PlaySound(SoundID.NPCHit36.WithVolumeScale(0.6f), Projectile.Center);
        }

        public override void OnKill(int timeLeft) {
            if (timeLeft <= 0) {
                SoundEngine.PlaySound(SoundID.NPCDeath39.WithVolumeScale(0.8f), Projectile.Center);
            }
            
            for (int i = 0; i < 15; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy);
            }
        }

    }
}
