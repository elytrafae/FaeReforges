using FaeReforges.Content.Items.TinkererHammers;
using FaeReforges.Systems.ReforgeHammers;
using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
            meteoriteType.onApplyWeapon = (item) => { item.crit += 10; item.damage = (int)Math.Ceiling(item.damage * 0.9); };
            meteoriteType.onUpdateAccessory = (item, player, visible) => { player.GetDamage(DamageClass.Generic) -= 0.1f; player.GetCritChance(DamageClass.Generic) += 10; };
            InitHammerTypeHelper<HellstoneTinkererHammer>(50, 120);

            // Hardmode
            InitHammerTypeHelper<CobaltTinkererHammer>(45, 120);
            InitHammerTypeHelper<PalladiumTinkererHammer>(50, 115);

            ReforgeHammerType mythrilType = InitHammerTypeHelper<MythrilTinkererHammer>(45, 115);
            mythrilType.onUpdateAccessory = (item, player, hideVisual) => { player.statDefense++; };

            ReforgeHammerType orichalcumType = InitHammerTypeHelper<OrichalcumTinkererHammer>(45, 115);
            orichalcumType.onUpdateAccessory = (item, player, hideVisual) => { player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement++; }; // I am just gonna highjack this because I'm lazy . . .

            ReforgeHammerType adamantiteType = InitHammerTypeHelper<AdamantiteTinkererHammer>(45, 110);
            adamantiteType.onUpdateAccessory = (item, player, hideVisual) => { player.GetDamage(DamageClass.Generic) += 0.01f; ; };

            ReforgeHammerType titaniumType = InitHammerTypeHelper<TitaniumTinkererHammer>(45, 110);
            titaniumType.onUpdateAccessory = (item, player, hideVisual) => { player.GetCritChance(DamageClass.Generic) += 1f; };

            ReforgeHammerType hallowedType = InitHammerTypeHelper<HallowedTinkererHammer>(40, 105);
            // TODO: Implement this

            ReforgeHammerType frostType = InitHammerTypeHelper<FrostTinkererHammer>(40, 105); // TODO: The effect is broken. Fix it! Maybe use += 0.1f?
            frostType.changeWeaponDealDamageNpc = (item, attacker, victim, hitModifiers) => { 
                if (victim.HasBuff(BuffID.Frostburn2)) {
                    hitModifiers.SourceDamage *= 1.1f;
                }
            };
            frostType.changeWeaponDealDamagePvp = (item, attacker, victim, hurtModifiers) => {
                if (victim.HasBuff(BuffID.Frostburn2)) {
                    hurtModifiers.SourceDamage *= 1.1f;
                }
            };

            // TODO: Continue, and make the sprites you lazy bum!

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

    }
}
