using FaeReforges.Content.Buffs;
using FaeReforges.Content.Items.PowerUps;
using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems;
using FaeReforges.Systems.ReforgeHammerContent;
using FaeReforges.Systems.ReforgeHammers;
using FaeReforges.Systems.VanillaReforges;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content {
    public class SystemToLoadAllHammerTypes : ModSystem {

        public override void OnModLoad() {
            /*
            // Prehardmode
            InitHammerTypeHelper<StoneTinkererHammer>(75, 150);
            InitHammerTypeHelper<CopperTinkererHammer>(70, 150);
            InitHammerTypeHelper<TinTinkererHammer>(75, 145);
            InitHammerTypeHelper<IronTinkererHammer>(65, 145);
            InitHammerTypeHelper<LeadTinkererHammer>(70, 140);
            InitHammerTypeHelper<SilverTinkererHammer>(60, 140);
            InitHammerTypeHelper<TungstenTinkererHammer>(65, 135);
            InitHammerTypeHelper<GoldenTinkererHammer>(55, 135);
            InitHammerTypeHelper<PlatinumTinkererHammer>(60, 130);

            InitHammerTypeHelper<DemoniteTinkererHammer>(50, 130);
            InitHammerTypeHelper<CrimtaneTinkererHammer>(55, 125);
            ReforgeHammerType meteoriteType = InitHammerTypeHelper<MeteoriteTinkererHammer>(53, 127);
            meteoriteType.onApplyWeapon = (Item item) => { item.crit += 10; item.damage = (int)Math.Ceiling(item.damage * 0.9); };
            meteoriteType.onUpdateAccessory = (Item item, Player player, bool visible) => { player.GetDamage(DamageClass.Generic) -= 0.1f; player.GetCritChance(DamageClass.Generic) += 10; };
            InitHammerTypeHelper<HellstoneTinkererHammer>(50, 120);
            */

            ReforgeHammerType meteoriteType = InitHammerTypeHelper<MeteoriteTinkererHammer>();
            meteoriteType.onApplyWeapon = (Item item) => { item.crit += 10; item.damage = (int)Math.Ceiling(item.damage * 0.9); };
            meteoriteType.onUpdateAccessory = (Item item, Player player, bool visible) => { player.GetDamage(DamageClass.Generic) -= 0.1f; player.GetCritChance(DamageClass.Generic) += 10; };

            // Hardmode
            ReforgeHammerType cobaltHammer = InitHammerTypeHelper<CobaltTinkererHammer>();
            cobaltHammer.onUpdateAccessory = (Item item, Player player, bool hideVisual) => {
                VanillaReforgePlayer mp = player.GetModPlayer<VanillaReforgePlayer>();
                mp.ammoSavePoints += 1;
                mp.shootVelocityPoints += 1;
                mp.manaPercentageRegenPerSecond += 1;
            };
            
            ReforgeHammerType palladiumHammer = InitHammerTypeHelper<PalladiumTinkererHammer>();
            palladiumHammer.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetAttackSpeed(DamageClass.Melee) += 0.01f; };

            ReforgeHammerType mythrilType = InitHammerTypeHelper<MythrilTinkererHammer>();
            mythrilType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.statDefense++; };

            ReforgeHammerType orichalcumType = InitHammerTypeHelper<OrichalcumTinkererHammer>();
            orichalcumType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement++; }; // I am just gonna highjack this because I'm lazy . . .

            ReforgeHammerType adamantiteType = InitHammerTypeHelper<AdamantiteTinkererHammer>();
            adamantiteType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetDamage(DamageClass.Generic) += 0.01f; ; };

            ReforgeHammerType titaniumType = InitHammerTypeHelper<TitaniumTinkererHammer>();
            titaniumType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetCritChance(DamageClass.Generic) += 1f; };

            ReforgeHammerType hallowedType = InitHammerTypeHelper<HallowedTinkererHammer>(); // Implemented elsewhere due to hooks

            ReforgeHammerType frostType = InitHammerTypeHelper<FrostTinkererHammer>(); // TODO: Test again
            frostType.changeWeaponDealDamageNpc = (int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => { 
                if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                    hitModifiers.SourceDamage += 0.1f;
                }
            };
            frostType.changeWeaponDealDamagePvp = (int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) => {
                if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                    hurtModifiers.SourceDamage += 0.1f;
                }
            };

            ReforgeHammerType forbiddenType = InitHammerTypeHelper<ForbiddenTinkererHammer>(); // TODO: Test
            forbiddenType.changeWeaponDealDamagePvp = (int item, Player attacker, Player victim, ref Player.HurtModifiers hurtInfo) => {
                hurtInfo.SourceDamage *= ForbiddenHammerDamageMultiplier(attacker);
            };
            forbiddenType.changeWeaponDealDamageNpc = (int item, Player attacker, NPC victim, ref NPC.HitModifiers hitInfo) => {
                hitInfo.SourceDamage *= ForbiddenHammerDamageMultiplier(attacker);
            };
            // The summoner part is somewhere else
            //forbiddenType.onApplyWeapon = (item) => { if (!item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) { item.mana += 10; item.damage += 15; } }; // TODO: Make Mana Sickness and Yoyos somehow not an issue!


            ReforgeHammerType chlorophyteType = InitHammerTypeHelper<ChlorophyteTinkererHammer>(); // TODO: Test
            chlorophyteType.onWeaponDealDamageNpc = (int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) => { attacker.AddBuff(ModContent.BuffType<ChlorophyteRejuvenation>(), 120); };
            chlorophyteType.onWeaponDealDamagePvp = (int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => { attacker.AddBuff(ModContent.BuffType<ChlorophyteRejuvenation>(), 120); };

            ReforgeHammerType venomiteType = InitHammerTypeHelper<VenomiteTinkererHammer>(); // TODO: Test
            venomiteType.onWeaponDealDamageNpc = (int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) => { victim.AddBuff(BuffID.Venom, 180); };
            venomiteType.onWeaponDealDamagePvp = (int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => { victim.AddBuff(BuffID.Venom, 180); };

            ReforgeHammerType spectreType = InitHammerTypeHelper<SpectreTinkererHammer>(); // TODO: Test
            spectreType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().dodgeChanceThousandth += 15; }; // +1.5% Dodge Chance

            ReforgeHammerType shroomiteType = InitHammerTypeHelper<ShroomiteTinkererHammer>(); // TODO: Test
            shroomiteType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().flightTimeThousandth += 25; };

            ReforgeHammerType solarType = InitHammerTypeHelper<SolarTinkererHammer>(); // TODO: Test
            solarType.onUpdateWeaponHeld = (Item item, Player player) => { player.dashType = 3; }; // We are using the solar flare dash, yes.
            solarType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().accessoryReforgedWithSolar = true; if (!player.HasBuff<SolarEclipse>()) { player.endurance += 0.015f; } }; // 1.5%

            ReforgeHammerType vortexType = InitHammerTypeHelper<VortexTinkererHammer>(); // TODO: Test
            vortexType.changeWeaponDealDamageNpc = (int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerVortexCrit(item)) {
                    hitModifiers.SetCrit();
                    hitModifiers.CritDamage += 1f;
                    // Send message to display stuff
                    ((FaeReforges)Mod).SendDisplayCombatText(victim, "CombatTexts.GuaranteedSuperCrit", Color.Red);
                }
            };
            /*
            vortexType.changeWeaponDealDamagePvp = (item, attacker, victim, hurtInfo) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerVortexCrit(item.type)) {
                    hurtInfo.
                }
            };
            */
            vortexType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement -= 1; player.GetDamage(DamageClass.Generic) += 0.02f; };

            ReforgeHammerType nebulaType = InitHammerTypeHelper<NebulaTinkererHammer>(); // TODO: Test
            nebulaType.onWeaponDealDamageNpc = (int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damage) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerNebulaBooster()) {
                    int boosterItem = Item.NewItem(new EntitySource_OnHit(attacker, victim), victim.Hitbox, ModContent.ItemType<SpeedBooster>(), 1);
                    if (Main.netMode == NetmodeID.MultiplayerClient && boosterItem >= 0) {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, boosterItem, 1f);
                    }
                }
            };
            nebulaType.onWeaponDealDamagePvp = (int item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerNebulaBooster()) {
                    int boosterItem = Item.NewItem(new EntitySource_OnHit(attacker, victim), victim.Hitbox, ModContent.ItemType<SpeedBooster>(), 1);
                    if (Main.netMode == NetmodeID.MultiplayerClient && boosterItem >= 0) {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, boosterItem, 1f);
                    }
                }
            };
            nebulaType.onUpdateAccessory = (Item item, Player player, bool visual) => {
                player.GetCritChance(DamageClass.Generic) += 0.03f;
                player.endurance -= 0.01f;
            };

            ReforgeHammerType stardustType = InitHammerTypeHelper<StardustTinkererHammer>(); // TODO: Test
            stardustType.changeWeaponDealDamageNpc = (int item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => {
                hitModifiers.SourceDamage *= 1f + (attacker.maxMinions + attacker.maxTurrets - (int)Math.Ceiling(CountMinionsAndSentries(attacker))) * 3 / 100f;
            };
            stardustType.changeWeaponDealDamagePvp = (int item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) => {
                hurtModifiers.SourceDamage *= 1f + (attacker.maxMinions + attacker.maxTurrets - (int)Math.Ceiling(CountMinionsAndSentries(attacker))) * 3 / 100f;
            };
            stardustType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetModPlayer<MyReforgeHammerPlayer>().bonusSentrySlotHundreth += 25; };
            /* // Old effects!
            stardustType.onApplyWeapon = (Item item) => { item.damage = (int)Math.Ceiling(item.damage * 2.4); item.useAnimation *= 2; item.useTime *= 2; };
            stardustType.onCreateProjectile = (int item, Projectile projectile, IEntitySource context) => { 
                projectile.localNPCHitCooldown = Math.Max(projectile.localNPCHitCooldown*2, 2); 
                projectile.idStaticNPCHitCooldown = Math.Max(projectile.idStaticNPCHitCooldown * 2, 2); 
            };
            stardustType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().stardustHammerAccessoryCount++; };
            */ 

            /*
            ReforgeHammerType testType = new ReforgeHammerType();
            testType.negativeReforgeChance = 100;
            testType.reforgeCost = 20;
            testType.onApplyWeapon = (item) => { item.damage += 100; item.crit += 50; };
            testType.onApplyAccessory = (item) => { item.defense += 100; };
            testType.onUpdateAccessory = (item, player, hideVisual) => { player.statLifeMax2 += 1000; };
            testType.onUpdateWeaponHeld = (item, player) => { player.AddBuff(BuffID.Regeneration, 60); };
            testType.changeWeaponDealDamagePvp = (item, attacker, victim, hurtModifiers) => { hurtModifiers.Knockback += 1000; };
            testType.onWeaponDealDamagePvp = (item, attacker, victim, hurtInfo) => { victim.AddBuff(BuffID.Venom, 300, false); };
            testType.changeWeaponDealDamageNpc = (item, attacker, victim, hitModifiers) => { hitModifiers.Knockback += 1000; };
            testType.onWeaponDealDamageNpc = (item, attacker, victim, hitInfo, damageDone) => { victim.AddBuff(BuffID.Venom, 300); };
            ReforgeHammerRegistry.RegisterHammerType(new Terraria.Item(ItemID.WoodenHammer), testType);
            */
        }

        private ReforgeHammerType InitHammerTypeHelper<T>() where T : ModItem {
            ReforgeHammerType hammerType = new ReforgeHammerType();
            //hammerType.negativeReforgeChance = negChance;
            //hammerType.reforgeCost = refCost;
            ReforgeHammerRegistry.RegisterHammerType(ModContent.GetInstance<T>().Item, hammerType);
            return hammerType;
        }

        /* Mod.Call test. Works perfectly!
        private dynamic InitHammerTypeHelper<T>(int negChance, int refCost) where T : ModItem {
            dynamic hammerType = Mod.Call("RegisterTinkererHammer", ModContent.GetInstance<T>().Item);
            hammerType.negativeReforgeChance = negChance;
            hammerType.reforgeCost = refCost;
            return hammerType;
        }
        */

        private static float ForbiddenHammerDamageMultiplier(Player player) {
            if (player.statLifeMax2 <= 0) {
                return 1f;
            }
            return 1f + (1f - ( ((float)player.statMana) / player.statManaMax2 ) ) * 0.15f;
        }

        private static double CountMinionsAndSentries(Player player) {
            double count = 0;
            foreach (var proj in Main.ActiveProjectiles) {
                if (proj.minion && proj.owner == Main.myPlayer) {
                    count += proj.minionSlots;
                }
                if (proj.WipableTurret) {
                    count += proj.GetGlobalProjectile<SummonerReforgesGlobalProjectile>().sentryOccupancy;
                }
            }
            return count;
        }

    }

    
}
