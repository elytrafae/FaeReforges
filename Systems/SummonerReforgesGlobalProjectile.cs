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
        public float bonusSpeed = 1f;
        public float excessUpdates = 0f;
        public float bonusTagEffectiveness = 1f;
        public float bonusArmorPen = 0f;
        public float reservedCumulativeSummonOccupancyFromEitherMyselfOrParent = 0f;

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

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            if (source is IEntitySource_WithStatsFromItem itemSource && itemSource.Item != null && itemSource.Item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) {
                bonusSpeed = globItem.summonSpeedMult;
                bonusTagEffectiveness = globItem.summonTagEffectiveness;
                bonusArmorPen = globItem.summonArmorPen;
            } else if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                var parentModProj = parentProj.GetGlobalProjectile<SummonerReforgesGlobalProjectile>();
                bonusTagEffectiveness = parentModProj.bonusTagEffectiveness;
                bonusArmorPen = parentModProj.bonusArmorPen;
                bonusSpeed = parentModProj.bonusSpeed;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            modifiers.ScalingArmorPenetration += bonusArmorPen;
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            modifiers.ScalingArmorPenetration += bonusArmorPen;
        }

    }
}
