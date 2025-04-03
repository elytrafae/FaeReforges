using FaeLibrary.API.ItemConditions;
using System;
using Terraria.ID;
using Terraria;
using FaeReforges.Systems.PrivatePickups;
using FaeReforges.Content.PrivatePickups;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier3 {
    internal class SquireTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 50);
        public override int HammerTier => 3;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsMeleeWeapon;

        int timer = 0; // No need to sync this LMAO :3:3:3
        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            if (player.whoAmI != Main.myPlayer) {
                return;
            }
            timer++;
            if (timer >= 120) {
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
                } while (tries > 0 && !properDir) ;

                PrivatePickupManager.Spawn<ShadowflamePickup>(x, y);
                SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot);
            }
        }

    }
}
