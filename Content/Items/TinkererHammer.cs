using FaeReforges.Systems.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Microsoft.Xna.Framework;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.Items {

    [Autoload(false)]
    public class TinkererHammer : ModItem {

        private Mod mod;
        private AbstractHammerType hammerType;

        public TinkererHammer(Mod mod, AbstractHammerType hammerType) {
            this.mod = mod;
            this.hammerType = hammerType;
        }

        protected override bool CloneNewInstances => true;
        public override string Name => hammerType.FullName;
        public override LocalizedText DisplayName => hammerType.DisplayName;
        //public override LocalizedText Tooltip => hammerType.Description;
        public override string Texture => hammerType.Texture;

        public override void SetDefaults() {
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item37;
        }

        // TODO: Add a system for keeping track of which hammers are unloacked and which ones aren't.
        // TODO: Make this hammer unlock itself upon consumption, and make it not consumable when it already is unlocked in that world.
        public override bool? UseItem(Player player) {
            return true;
        }

        public override bool ConsumeItem(Player player) {
            return !ReforgeHammerSaveSystem.IsHammerUnlocked(hammerType);
        }

        public override void OnConsumeItem(Player player) {
            ReforgeHammerSaveSystem.UnlockHammer(hammerType);
            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(AbstractHammerType.ReforgeUnlockMessage.Key, hammerType.Name), Color.LightGray);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            int consumableIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Consumable");
            int materialIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Material");
            int tooltipIndex = tooltips.FindIndex(tooltip => tooltip.Name == "Tooltip0");
            int insertIndex = Math.Max(consumableIndex, materialIndex);
            if (insertIndex == -1) {
                if (tooltipIndex != -1) {
                    insertIndex = tooltipIndex - 1;
                } else { 
                    insertIndex = tooltips.Count - 1;
                }
            }

            TooltipLineHelper("TinkererHammerCost", AbstractHammerType.CostTooltip.Format(hammerType.reforgeCost), ref tooltips, ref insertIndex);
            TooltipLineHelper("TinkererHammerNegativeChance", AbstractHammerType.NegativeReforgeChanceTooltip.Format(hammerType.negativeReforgeChance), ref tooltips, ref insertIndex);
            if (hammerType.WeaponEffect.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerWeaponEffect", AbstractHammerType.WeaponEffectPrefix.Format(hammerType.WeaponEffect), ref tooltips, ref insertIndex);
            }
            if (hammerType.AccessoryEffect.Value.Length > 0) {
                TooltipLineHelper("TinkererHammerAccessoryEffect", AbstractHammerType.AccessoryEffectPrefix.Format(hammerType.AccessoryEffect), ref tooltips, ref insertIndex);
            }
            TooltipLineHelper("TinkererHammerTutorial", AbstractHammerType.TutorialTooltip.Value, ref tooltips, ref insertIndex);
        }

        private void TooltipLineHelper(string name, string text, ref List<TooltipLine> tooltips, ref int index) {
            tooltips.Insert(index, new TooltipLine(Mod, name, text));
            index++;
        }

        /*
        // I don't want you to load on your own, Tinkerer Hammer! I will load you when I want to!
        public class DynamicLoader : ILoadable {
            public void Load(Mod mod) {
            }

            public void Unload() {
            }
        }
        */

    }
}
