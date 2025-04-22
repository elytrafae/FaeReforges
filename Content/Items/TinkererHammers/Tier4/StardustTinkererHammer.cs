using FaeLibrary.API.ItemConditions;
using FaeReforges.Systems.ReforgeHammerContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using FaeReforges.Content.Cooldowns;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class StardustTinkererHammer : SimpleTinkererHammerItem {
        public const float SUMMON_RUSH_SPEED = 5;
        public const int BRIEF_MOMENT_DURATION = 30;
        public override int Rarity => ItemRarityID.Red;
        public override ItemCondition ReforgeableCondition => ItemCondition.GrammaticalAnd3(ItemCondition.IsMinionWeapon, ItemCondition.IsSentryWeapon, ItemCondition.IsAccessory);
        public override int HammerTier => 4;

        public override LocalizedText WeaponEffectText => base.WeaponEffectText.WithFormatArgs(SUMMON_RUSH_SPEED, ModContent.GetInstance<StardustHammerCooldown>().DisplayCooldownTicks/60f, ModContent.GetInstance<StardustHammerCooldown>().Charges);
        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (count % 5 == 0) {
                player.maxMinions++;
                player.maxTurrets++;
            }
        }

        // Hammer effect implemented elsewhere
    }
}
