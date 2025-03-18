using FaeReforges.Systems;
using FaeReforges.Systems.Config;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaeReforges.Content.Reforges {
    public class SummonerPrefixTemplate : ModPrefix {
        

        readonly string name;
        readonly int tier;
        readonly float damage;
        readonly float knockback;
        readonly float speed;
        readonly float effectiveness;
        readonly float armorpen;

        public SummonerPrefixTemplate(string name, int tier, float damage, float knockback, float speed, float effectiveness, float armorpen) { 
            this.name = name + "SummonerPrefix";
            this.tier = tier;
            this.damage = damage;
            this.knockback = knockback;
            this.speed = speed;
            this.effectiveness = effectiveness;
            this.armorpen = armorpen;
        }

        public override string Name => name;

        /*
        public override bool IsLoadingEnabled(Mod mod) {
            return ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges;
        }
        */

        public override PrefixCategory Category => PrefixCategory.Custom;

        public override bool CanRoll(Item item) {
            return ItemID.Sets.StaffMinionSlotsRequired[item.type] > 0;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {
            damageMult = 1f + damage;
            knockbackMult = 1f + knockback;
        }

        public override void Apply(Item item) {
            if (item.TryGetGlobalItem(out SummonerReforgesGlobalItem globItem)) {
                globItem.summonSpeedMult = 1f + speed;
                globItem.summonTagEffectiveness = 1f + effectiveness;
                globItem.summonArmorPen = armorpen;

            }
        }

        public override bool AllStatChangesHaveEffectOn(Item item) {
            return item.TryGetGlobalItem(out SummonerReforgesGlobalItem _);
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {

            if (speed != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerSpeed", SummonSpeedBuffTooltip.Format(speed * 100)) {
                    IsModifier = true,
                    IsModifierBad = speed < 0
                };
            }
            if (effectiveness != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerTagEffectiveness", SummonTagEffectivenessTooltip.Format(effectiveness * 100)) {
                    IsModifier = true,
                    IsModifierBad = effectiveness < 0
                };
            }
            if (armorpen != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerArmorPenetration", SummonArmorPenetrationTooltip.Format(armorpen * 100)) {
                    IsModifier = true,
                    IsModifierBad = armorpen < 0
                };
            }
        }

        public override void ModifyValue(ref float valueMult) {
            valueMult = ReforgeTierSystem.GetValueMult(tier);
        }

        public static LocalizedText SummonSpeedBuffTooltip { get; private set; }
        public static LocalizedText SummonTagEffectivenessTooltip { get; private set; }
        public static LocalizedText SummonArmorPenetrationTooltip { get; private set; }

        public override void SetStaticDefaults() {
            SummonSpeedBuffTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonSpeedBuffTooltip)}");
            SummonTagEffectivenessTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonTagEffectivenessTooltip)}");
            SummonArmorPenetrationTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonArmorPenetrationTooltip)}");
        }
    }
}
