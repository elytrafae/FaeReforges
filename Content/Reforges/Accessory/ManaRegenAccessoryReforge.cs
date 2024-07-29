using FaeReforges.Systems.VanillaReforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges.Accessory
{
    public class ManaRegenAccessoryReforge : ModPrefix
    {

        public class ReforgeLoader : ILoadable
        {
            public void Load(Mod mod) {
                mod.AddContent(new ManaRegenAccessoryReforge("Charmed", 4));
                // Arcane would go here
                mod.AddContent(new ManaRegenAccessoryReforge("Hexxed", 2));
                mod.AddContent(new ManaRegenAccessoryReforge("Jinxed", 1));
            }

            public void Unload() {
            }
        }

        readonly int power;
        readonly string name;

        public override string Name => name;
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public ManaRegenAccessoryReforge(string name, int power)
        {
            this.name = name;
            this.power = power;
        }

        public override void ApplyAccessoryEffects(Player player)
        {
            player.GetModPlayer<VanillaReforgePlayer>().manaPercentageRegenPerSecond += power;
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= VanillaReforgePlayer.ACCESSORY_VALUES_PER_POWER[power];
        }
        

        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "PrefixMaxManaRegen", RegenTooltip.Format(power))
            {
                IsModifier = true,
                IsModifierBad = power < 0
            };
        }

        public static LocalizedText RegenTooltip { get; private set; }

        public override void SetStaticDefaults()
        {
            RegenTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(RegenTooltip)}");
        }

    }
}
