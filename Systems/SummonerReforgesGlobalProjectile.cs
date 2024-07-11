using FaeReforges.Systems.Config;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace FaeReforges.Systems
{
    public class SummonerReforgesGlobalProjectile : GlobalProjectile
    {

        public override bool InstancePerEntity => true;
        public float sentryOccupancy = 0f;
        public float initOccupancy = 0f;
        public float bonusSpeed = 1f;
        public float excessUpdates = 0f;
        public int reforgedCritChance = 0;

        // Do not add this back!
        /*
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
            return initOccupancy > 0;
        }
        */

        public override void SetDefaults(Projectile entity) {
            if (entity.type <= 0 || entity.type >= ProjectileID.Count) {
                return;
            }
            if (entity.minionSlots > 0 || entity.sentry) {
                if (ModContent.GetInstance<ServerConfig>().MakeAllMinionsHaveLocalIFrames) {
                    if (ModContent.GetInstance<ServerConfig>().MinionsThatShouldHaveStaticIFrames.Contains(new ProjectileDefinition(entity.type))) {
                        if (entity.usesLocalNPCImmunity && !entity.usesIDStaticNPCImmunity) {
                            entity.usesIDStaticNPCImmunity = true;
                            entity.usesLocalNPCImmunity = false;
                            entity.idStaticNPCHitCooldown = entity.localNPCHitCooldown;
                        }
                    } else {
                        if (!entity.usesLocalNPCImmunity && entity.usesIDStaticNPCImmunity) {
                            entity.usesIDStaticNPCImmunity = false;
                            entity.usesLocalNPCImmunity = true;
                            entity.localNPCHitCooldown = entity.idStaticNPCHitCooldown;
                        }
                    }
                }
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            initOccupancy = projectile.minionSlots;
            if (source is IEntitySource_WithStatsFromItem itemSource && itemSource.Item != null && itemSource.Item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) {
                if (projectile.sentry) {
                    sentryOccupancy = globItem.minionOccupancyMult;
                }
                projectile.minionSlots = initOccupancy * globItem.minionOccupancyMult; // In case a projectile is marked both as a minion and a sentry
                bonusSpeed = globItem.minionSpeedMult;
                reforgedCritChance = globItem.minionCritBonus;
            } else if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                reforgedCritChance = parentProj.GetGlobalProjectile<SummonerReforgesGlobalProjectile>().reforgedCritChance;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            if (Main.rand.Next(100) < reforgedCritChance) {
                modifiers.SetCrit();
            }
        }

    }
}
