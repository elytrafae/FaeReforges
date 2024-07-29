using FaeReforges.Content.Reforges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.VanillaReforges {
    public class VanillaAccessoryReforge : ModSystem {

        public override void Load() {
            On_Player.GrantPrefixBenefits += On_Player_GrantPrefixBenefits;
        }

        private void On_Player_GrantPrefixBenefits(On_Player.orig_GrantPrefixBenefits orig, Player self, Item item) {
            VanillaReforgePlayer modPlayer = self.GetModPlayer<VanillaReforgePlayer>();
            switch (item.prefix) {
                case PrefixID.Arcane:
                    modPlayer.manaPercentageRegenPerSecond += 3;
                    break;

                case PrefixID.Quick2:
                    modPlayer.accessoryMovement += 4;
                    break;

                case PrefixID.Hasty2:
                    modPlayer.accessoryMovement += 3;
                    break;

                case PrefixID.Fleeting:
                    modPlayer.accessoryMovement += 2;
                    break;

                case PrefixID.Brisk:
                    modPlayer.accessoryMovement += 1;
                    break;

                default:
                    orig(self, item);
                    break;
            }
        }

    }
}
