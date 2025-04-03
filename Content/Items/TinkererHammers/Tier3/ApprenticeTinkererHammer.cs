using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.PrivatePickups;
using FaeReforges.Systems.PrivatePickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    internal class ApprenticeTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 50);
        public override int HammerTier => 3;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsMagicWeapon;

        int timer = 0; // No need to sync this LMAO :3:3:3
        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            if (player.whoAmI != Main.myPlayer) {
                return;
            }
            timer++;
            if (timer >= 150) {
                timer = 0;
                bool properDir;
                int tries = 10;
                double dir;
                int x;
                int y;
                do {
                    dir = Main.rand.NextFloat() * Math.PI * 2;
                    x = (int)(player.Center.X + Math.Cos(dir) * 400);
                    y = (int)(player.Center.Y + Math.Sin(dir) * 400);
                    Point tileLocation = new Vector2(x, y).ToTileCoordinates();
                    properDir = !(Main.tile[tileLocation].HasTile && Main.tile[tileLocation].HasUnactuatedTile);
                    tries--;
                } while (tries > 0 && !properDir);

                PrivatePickupManager.Spawn<ManaStarPickup>(x, y);
                SoundEngine.PlaySound(SoundID.Item29);
            }
        }

    }
}
