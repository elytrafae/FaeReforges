using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.WhipFrenzy {
    internal class WhipFrenzyGlobalItem : GlobalItem {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
            return WhipFrenzyPlayer.IsEligibleWhip(entity);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.WhipFrenzyPlayer().IsFrenzyActive() && Main.myPlayer == player.whoAmI) {
                int additionalWhipUsesLeft = 2;
                int[] whipsUsedSoFar = new int[additionalWhipUsesLeft];
                for (var i = 0; i <= 9; i++) { // Please find a better way to iterate through hotbar items
                    Item hotbarItem = player.inventory[i];
                    if (WhipFrenzyPlayer.IsEligibleWhip(hotbarItem) && !whipsUsedSoFar.Contains(hotbarItem.type) && hotbarItem.type != item.type) {
                        Console.WriteLine("Additional Whip Found! " + hotbarItem.AffixName());
                        whipsUsedSoFar[whipsUsedSoFar.Length - additionalWhipUsesLeft] = hotbarItem.type;
                        additionalWhipUsesLeft--;
                        int projectile = Projectile.NewProjectile(
                            player.GetSource_ItemUse(hotbarItem),
                            position,
                            velocity,
                            hotbarItem.shoot,
                            (int)player.GetDamage(hotbarItem.DamageType).ApplyTo(hotbarItem.damage),
                            player.GetKnockback(hotbarItem.DamageType).ApplyTo(hotbarItem.knockBack),
                            player.whoAmI
                            );
                        if (additionalWhipUsesLeft <= 0) {
                            break;
                        }
                    }
                }

            }
            return true;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            
        }

    }
}
