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

            // Hardmode
            InitHammerTypeHelper<CobaltTinkererHammer>(45, 120);
            InitHammerTypeHelper<PalladiumTinkererHammer>(50, 115);

            ReforgeHammerType mythrilType = InitHammerTypeHelper<MythrilTinkererHammer>(45, 115);
            mythrilType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.statDefense++; };

            ReforgeHammerType orichalcumType = InitHammerTypeHelper<OrichalcumTinkererHammer>(45, 115);
            orichalcumType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement++; }; // I am just gonna highjack this because I'm lazy . . .

            ReforgeHammerType adamantiteType = InitHammerTypeHelper<AdamantiteTinkererHammer>(45, 110);
            adamantiteType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetDamage(DamageClass.Generic) += 0.01f; ; };

            ReforgeHammerType titaniumType = InitHammerTypeHelper<TitaniumTinkererHammer>(45, 110);
            titaniumType.onUpdateAccessory = (Item item, Player player, bool hideVisual) => { player.GetCritChance(DamageClass.Generic) += 1f; };

            ReforgeHammerType hallowedType = InitHammerTypeHelper<HallowedTinkererHammer>(40, 105); // Implemented elsewhere due to hooks

            ReforgeHammerType frostType = InitHammerTypeHelper<FrostTinkererHammer>(40, 105); // TODO: Test again
            frostType.changeWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => { 
                if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                    hitModifiers.SourceDamage += 0.1f;
                }
            };
            frostType.changeWeaponDealDamagePvp = (Item item, Player attacker, Player victim, ref Player.HurtModifiers hurtModifiers) => {
                if (victim.HasBuff(BuffID.Frostburn) || victim.HasBuff(BuffID.Frostburn2)) {
                    hurtModifiers.SourceDamage += 0.1f;
                }
            };

            ReforgeHammerType forbiddenType = InitHammerTypeHelper<ForbiddenTinkererHammer>(40, 105); // TODO: Test
            forbiddenType.canUseItem = (Item item, Player player) => { return player.statMana >= 10 || item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem); };
            forbiddenType.changeWeaponDealDamagePvp = (Item item, Player attacker, Player victim, ref Player.HurtModifiers hurtInfo) => { 
                if (!item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) { 
                    if (attacker.HasBuff(BuffID.ManaSickness) && !item.CountsAsClass(DamageClass.Magic)) {
                        hurtInfo.FinalDamage *= 0.5f;
                    }
                    if (attacker.statMana >= 10) {
                        attacker.statMana -= 10;
                        hurtInfo.SourceDamage.Base += 20;
                        attacker.manaRegenDelay = attacker.maxRegenDelay;
                    }
                } 
            };
            forbiddenType.changeWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, ref NPC.HitModifiers hitInfo) => {
                if (!item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) {
                    if (attacker.HasBuff(BuffID.ManaSickness) && !item.CountsAsClass(DamageClass.Magic)) {
                        hitInfo.FinalDamage *= 0.5f;
                    }
                    if (attacker.statMana >= 10) {
                        attacker.statMana -= 10;
                        hitInfo.SourceDamage.Base += 20;
                        attacker.manaRegenDelay = attacker.maxRegenDelay;
                    }
                }
            };
            // The summoner part is somewhere else
            //forbiddenType.onApplyWeapon = (item) => { if (!item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) { item.mana += 10; item.damage += 15; } }; // TODO: Make Mana Sickness and Yoyos somehow not an issue!


            ReforgeHammerType chlorophyteType = InitHammerTypeHelper<ChlorophyteTinkererHammer>(40, 95); // TODO: Test
            chlorophyteType.onWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) => { attacker.AddBuff(ModContent.BuffType<ChlorophyteRejuvenation>(), 120); };
            chlorophyteType.onWeaponDealDamagePvp = (Item item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => { attacker.AddBuff(ModContent.BuffType<ChlorophyteRejuvenation>(), 120); };

            ReforgeHammerType venomiteType = InitHammerTypeHelper<VenomiteTinkererHammer>(40, 95); // TODO: Test
            venomiteType.onWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) => { victim.AddBuff(BuffID.Venom, 180); };
            venomiteType.onWeaponDealDamagePvp = (Item item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => { victim.AddBuff(BuffID.Venom, 180); };

            ReforgeHammerType spectreType = InitHammerTypeHelper<SpectreTinkererHammer>(40, 95); // TODO: Test
            spectreType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().dodgeChanceThousandth += 15; }; // +1.5% Dodge Chance

            ReforgeHammerType shroomiteType = InitHammerTypeHelper<ShroomiteTinkererHammer>(40, 95); // TODO: Test
            shroomiteType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().flightTimeThousandth += 25; };

            ReforgeHammerType solarType = InitHammerTypeHelper<SolarTinkererHammer>(30, 80); // TODO: Test
            solarType.onUpdateWeaponHeld = (Item item, Player player) => { player.dashType = 3; }; // We are using the solar flare dash, yes.
            solarType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().accessoryReforgedWithSolar = true; if (!player.HasBuff<SolarEclipse>()) { player.endurance += 0.015f; } }; // 1.5%

            ReforgeHammerType vortexType = InitHammerTypeHelper<VortexTinkererHammer>(30, 80); // TODO: Test
            vortexType.changeWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, ref NPC.HitModifiers hitModifiers) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerVortexCrit(item.type)) {
                    hitModifiers.SetCrit();
                    Rectangle rectangle = new Rectangle((int)victim.Center.X + (int)Main.rand.NextFloat(-20, 20), (int)victim.position.Y + 30, 50, 50);
                    CombatText.NewText(rectangle, Color.Red, Mod.GetLocalization("CombatTexts.GuaranteedCrit").Value, true);
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

            ReforgeHammerType nebulaType = InitHammerTypeHelper<NebulaTinkererHammer>(30, 80); // TODO: Test
            nebulaType.onWeaponDealDamageNpc = (Item item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damage) => {
                if (attacker.GetModPlayer<MyReforgeHammerPlayer>().TriggerNebulaBooster()) {
                    int boosterItem = Item.NewItem(new EntitySource_OnHit(attacker, victim), victim.Hitbox, ModContent.ItemType<SpeedBooster>(), 1);
                    if (Main.netMode == NetmodeID.MultiplayerClient && boosterItem >= 0) {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, boosterItem, 1f);
                    }
                }
            };
            nebulaType.onWeaponDealDamagePvp = (Item item, Player attacker, Player victim, Player.HurtInfo hurtInfo) => {
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

            ReforgeHammerType stardustType = InitHammerTypeHelper<StardustTinkererHammer>(30, 80); // TODO: Test
            stardustType.onApplyWeapon = (Item item) => { item.damage = (int)Math.Ceiling(item.damage * 2.4); item.useAnimation *= 2; item.useTime *= 2; };
            stardustType.onCreateProjectile = (Item item, Projectile projectile, IEntitySource context) => { 
                projectile.localNPCHitCooldown = Math.Max(projectile.localNPCHitCooldown*2, 2); 
                projectile.idStaticNPCHitCooldown = Math.Max(projectile.idStaticNPCHitCooldown * 2, 2); 
            };
            stardustType.onUpdateAccessory = (Item item, Player player, bool visual) => { player.GetModPlayer<MyReforgeHammerPlayer>().stardustHammerAccessoryCount++; };

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

        private ReforgeHammerType InitHammerTypeHelper<T>(int negChance, int refCost) where T : ModItem {
            ReforgeHammerType hammerType = new ReforgeHammerType();
            hammerType.negativeReforgeChance = negChance;
            hammerType.reforgeCost = refCost;
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

        // TODO: Stardardize reforge values!

    }
}
