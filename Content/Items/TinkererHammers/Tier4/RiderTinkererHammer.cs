using FaeLibrary.API.ItemConditions;
using Terraria.ID;
using Terraria;
using FaeLibrary.API.ClassExtensions;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class RiderTinkererHammer : SimpleTinkererHammerItem {
        const float SPEED_BUFF = 0.025f;
        const float ACCEL_BUFF = 0.05f;

        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(SPEED_BUFF * 100, ACCEL_BUFF * 100);
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            player.GetMountAcceleration() += ACCEL_BUFF;
            player.GetMountDashSpeed() += SPEED_BUFF;
            player.GetMountRunSpeed() += SPEED_BUFF;
        }
    }
}
