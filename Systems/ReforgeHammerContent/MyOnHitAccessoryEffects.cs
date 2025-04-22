using FaeLibrary.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammerContent {

    public class MyOnHitAccessoryEffects {
        public static void JointModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers, Entity itemOrProjectile) {
            MyReforgeHammerPlayer2 myPlayer = MyReforgeHammerPlayer2.Get(player);
            modifiers.ScalingArmorPenetration += (myPlayer.hammerOfMightCount * 0.03f);
            modifiers.ModifyHitInfo += HammerOfSight_ModifyHurtInfo(target, myPlayer, modifiers.SuperArmor);
            if (DoIHaveAHigherHealthPercentage(player, target)) {
                modifiers.SourceDamage += 0.02f * myPlayer.terraHammerCount;
            }
        }

        public static void JointModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers, Entity itemOrProjectile) {
            MyReforgeHammerPlayer2 myPlayer = MyReforgeHammerPlayer2.Get(player);
            modifiers.ScalingArmorPenetration += (myPlayer.hammerOfMightCount * 0.03f);
            // Players cannot receive crits, therefore the Hammer of Sight does nothing!
            if (DoIHaveAHigherHealthPercentage(player, target)) {
                modifiers.SourceDamage += 0.02f * myPlayer.terraHammerCount;
            }
        }

        public static void JointOnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone, Entity itemOrProjectile) { 
            
        }

        public static void JointOnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo, Entity itemOrProjectile) { 
            
        }

        public static void JointModifyHitByNPC(Player player, NPC aggressor, ref Player.HurtModifiers modifiers, Projectile? projectile) {
            MyReforgeHammerPlayer2 myPlayer = MyReforgeHammerPlayer2.Get(player);
            if (!DoIHaveAHigherHealthPercentage(player, aggressor)) {
                modifiers.SourceDamage -= 0.02f * myPlayer.terraHammerCount;
            }
        }

        public static void JointModifyHitByPlayer(Player player, Player aggressor, ref Player.HurtModifiers modifiers, Entity itemOrProjectile) {
            MyReforgeHammerPlayer2 myPlayer = MyReforgeHammerPlayer2.Get(player);
            if (!DoIHaveAHigherHealthPercentage(player, aggressor)) {
                modifiers.SourceDamage -= 0.02f * myPlayer.terraHammerCount;
            }
        }

        public static void JointOnHitByNPC(Player player, NPC aggressor, Player.HurtInfo hurtInfo, Projectile? projectile) {

        }

        public static void JointOnHitByPlayer(Player player, Player aggressor, Player.HurtInfo hurtInfo, Entity itemOrProjectile) {

        }


        public static NPC.HitModifiers.HitInfoModifier HammerOfSight_ModifyHurtInfo(NPC target, MyReforgeHammerPlayer2 myPlayer, bool superArmor) {
            void modDelegate(ref NPC.HitInfo info) {
                if (info.Crit && !superArmor) {
                    info.Damage += (int)(target.defense * myPlayer.hammerOfSightCount * 0.04f);
                }
            }
            return modDelegate;
        }

        public static bool DoIHaveAHigherHealthPercentage(Player me, Entity other) {
            float myHealthRatio = (float)me.statLife / me.statLifeMax2;
            float otherHealthRatio = 0;
            if (other is Player otherPlayer) {
                otherHealthRatio = (float)otherPlayer.statLife / otherPlayer.statLifeMax2;
            } else if (other is NPC otherNPC) {
                otherNPC.GetLifeStats(out int otherHP, out int otherMaxHP);
                otherHealthRatio = (float)otherHP / otherMaxHP;
            }
            return myHealthRatio > otherHealthRatio;
        }

    }


    // Confusing name, but this GlobalItem will apply to WEAPONS hitting enemies and reacting to Accessory Effects
    public class MyOnHitAccessoryItem : GlobalItem {

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            MyOnHitAccessoryEffects.JointModifyHitNPC(player, target, ref modifiers, item);
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers) {
            MyOnHitAccessoryEffects.JointModifyHitPvp(player, target, ref modifiers, item);
            MyOnHitAccessoryEffects.JointModifyHitByPlayer(target, player, ref modifiers, item);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            MyOnHitAccessoryEffects.JointOnHitNPC(player, target, hit, damageDone, item);
        }

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            MyOnHitAccessoryEffects.JointOnHitPvp(player, target, hurtInfo, item);
        }

    }

    public class MyOnHitAccessoryProjectile : GlobalProjectile {

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            if (projectile.TryGetOwner(out Player owner)) {
                MyOnHitAccessoryEffects.JointModifyHitNPC(owner, target, ref modifiers, projectile);
            }
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            if (projectile.TryGetOwner(out Player owner)) {
                MyOnHitAccessoryEffects.JointModifyHitPvp(owner, target, ref modifiers, projectile);
                MyOnHitAccessoryEffects.JointModifyHitByPlayer(target, owner, ref modifiers, projectile);
            } else if (projectile.TryGetSourceNPC(out NPC npc)) {
                MyOnHitAccessoryEffects.JointModifyHitByNPC(target, npc, ref modifiers, projectile);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            if (projectile.TryGetOwner(out Player owner)) {
                MyOnHitAccessoryEffects.JointOnHitNPC(owner, target, hit, damageDone, projectile);
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            if (projectile.TryGetOwner(out Player owner)) {
                MyOnHitAccessoryEffects.JointOnHitPvp(owner, target, info, projectile);
                MyOnHitAccessoryEffects.JointOnHitByPlayer(target, owner, info, projectile);
            } else if (projectile.TryGetSourceNPC(out NPC npc)) { 
                MyOnHitAccessoryEffects.JointOnHitByNPC(target, npc, info, projectile);
            }
        }

    }

    public class MyOnHitAccessoryNPC : GlobalNPC {

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
            MyOnHitAccessoryEffects.JointModifyHitByNPC(target, npc, ref modifiers, null);
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
            MyOnHitAccessoryEffects.JointOnHitByNPC(target, npc, hurtInfo, null);
        }
    } 
}
