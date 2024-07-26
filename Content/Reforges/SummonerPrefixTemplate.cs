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

        /*
        public class DynamicSummonerPrefixLoader : ILoadable {
            public void Load(Mod mod) {
                if (!ModContent.GetInstance<ServerConfig>().EnableCustomSummonerReforges) {
                    return;
                }
                string json = Encoding.UTF8.GetString(mod.GetFileBytes("Assets/BalancingData/Reforges.json"));
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                JArray reforges = jsonObj.Summons;
                for (int i = 0; i < reforges.Count; i++) { 
                    dynamic reforge = reforges[i];
                    string name = reforge["Modifier Name"];
                    float damage = GetOrDefault<float>(reforge, "Damage", 0f); // TODO: Continue this
                    float knockback = GetOrDefault<float>(reforge, "Knockback", 0f);
                    float frenzy = GetOrDefault<float>(reforge, "Whip Frenzy Charge", 0f);
                    int crit = (int)(GetOrDefault<float>(reforge, "Crit", 0)*100);
                    float speed = GetOrDefault<float>(reforge, "Speed", 0);
                    float cost = GetOrDefault<float>(reforge, "Occupancy Reduction", 0);
                    bool positive = reforge.Positive;
                    mod.AddContent(new AbstractSummonerPrefix(name, positive, damage, knockback, crit, speed, cost, frenzy));
                }
            }

            public void Unload() {
            }
        }
        */
        

        readonly string name;
        readonly bool positive;
        readonly float damage;
        readonly float knockback;
        readonly int crit;
        readonly float speed;
        readonly float cost;
        readonly float frenzy;
        

        public SummonerPrefixTemplate(string name, bool positive, float damage, float knockback, int crit, float speed, float cost, float frenzy) { 
            this.name = name + "SummonerPrefix";
            this.positive = positive;
            this.damage = damage;
            this.knockback = knockback;
            this.crit = crit;
            this.speed = speed;
            this.cost = cost;
            this.frenzy = frenzy;
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
            if (item.TryGetGlobalItem<SummonerReforgesGlobalItem>(out SummonerReforgesGlobalItem globItem)) {
                globItem.minionOccupancyMult = 1f - cost;
                globItem.minionSpeedMult = 1f + speed;
                globItem.minionCritBonus = crit;
                globItem.whipFrenzyChargeMult = 1f + frenzy;
            }
        }

        public override bool AllStatChangesHaveEffectOn(Item item) {
            return item.TryGetGlobalItem(out SummonerReforgesGlobalItem _);
        }

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item) {
            /*
            if (MinionOccupancyReduction > 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyReductionTooltip.Format(MinionOccupancyReduction * 100)) {
                    IsModifier = true, // Sets the color to the positive modifier color.
                    IsModifierBad = false
                };
            } else if (MinionOccupancyReduction < 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyIncreaseTooltip.Format(-MinionOccupancyReduction * 100)) {
                    IsModifier = true, // Sets the color to the positive modifier color.
                    IsModifierBad = true
                };
            }
            */

            if (cost != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerOccupancy", SummonOccupancyTooltip.Format(-cost * 100)) {
                    IsModifier = true,
                    IsModifierBad = cost < 0
                };
            }


            if (speed != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerSpeed", SummonSpeedBuffTooltip.Format(speed * 100)) {
                    IsModifier = true,
                    IsModifierBad = speed < 0
                };
            }

            if (crit != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerCrit", SummonCritChanceTooltip.Format(crit)) {
                    IsModifier = true,
                    IsModifierBad = crit < 0
                };
            }

            if (frenzy != 0) {
                yield return new TooltipLine(Mod, "PrefixSummonerFrenzy", SummonFrenzyChargeTooltip.Format(frenzy * 100)) {
                    IsModifier = true,
                    IsModifierBad = frenzy < 0
                };
            }
            // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
            /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
        }

        public override void ModifyValue(ref float valueMult) {
            valueMult = positive ? 1.5f : 0.5f;
        }

        public static LocalizedText SummonOccupancyTooltip { get; private set; }
        //public static LocalizedText SummonOccupancyIncreaseTooltip { get; private set; }
        public static LocalizedText SummonSpeedBuffTooltip { get; private set; }
        public static LocalizedText SummonCritChanceTooltip { get; private set; }
        public static LocalizedText SummonFrenzyChargeTooltip { get; private set; }

        public override void SetStaticDefaults() {
            SummonOccupancyTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonOccupancyTooltip)}");
            //SummonOccupancyIncreaseTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonOccupancyIncreaseTooltip)}");
            SummonSpeedBuffTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonSpeedBuffTooltip)}");
            SummonCritChanceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonCritChanceTooltip)}");
            SummonFrenzyChargeTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(SummonFrenzyChargeTooltip)}");
        }
    }
}
