using FaeReforges.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.PowerUps {
    public class SpeedBooster : ModItem {

        const int BUFF_DURATION = 480; // 8 seconds

        public override void SetStaticDefaults() {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            //ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
            ItemID.Sets.IsAPickup[Item.type] = true;
        }

        public override void SetDefaults() {
            Item.width = 14;
            Item.height = 22;
        }

        // Saturation: 63, Lightness: 67
        public override bool OnPickup(Player player) {
            if (player.whoAmI != Main.myPlayer) {
                return false;
            }
            SoundEngine.PlaySound(SoundID.Grab);
            if (player.HasBuff<SpeedNebula3>()) {
                player.AddBuff(ModContent.BuffType<SpeedNebula3>(), BUFF_DURATION);
            } else if (player.HasBuff<SpeedNebula2>()) {
                player.ClearBuff(ModContent.BuffType<SpeedNebula2>());
                player.AddBuff(ModContent.BuffType<SpeedNebula3>(), BUFF_DURATION);
            } else if (player.HasBuff<SpeedNebula1>()) {
                player.ClearBuff(ModContent.BuffType<SpeedNebula1>());
                player.AddBuff(ModContent.BuffType<SpeedNebula2>(), BUFF_DURATION);
            } else {
                player.AddBuff(ModContent.BuffType<SpeedNebula1>(), BUFF_DURATION);
            }
            return false;
        }

        public override void PostUpdate() {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void GrabRange(Player player, ref int grabRange) {
            grabRange += 100;
        }

    }
}
