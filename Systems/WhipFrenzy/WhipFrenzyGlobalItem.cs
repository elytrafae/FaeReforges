using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Systems.WhipFrenzy {
    internal class WhipFrenzyGlobalItem : GlobalItem {

        public override bool? UseItem(Item item, Player player) {
            if (player.altFunctionUse == 2) {
                player.WhipFrenzyPlayer().ActivateFrenzy();
                return false;
            }

            /*
            int projectile = Projectile.NewProjectile(
                            player.GetSource_ItemUse(item),
                            player.Center,
                            Vector2.Zero,
                            ProjectileID.CoolWhip,
                            (int)player.GetDamage(item.DamageType).ApplyTo(item.damage),
                            player.GetKnockback(item.DamageType).ApplyTo(item.knockBack),
                            player.whoAmI,
                            ProjAIStyleID.Whip
                            );*/

            
            // When a player uses a whip, use two other ones!
            if (player.WhipFrenzyPlayer().IsFrenzyActive() && Main.myPlayer == player.whoAmI) {
                int additionalWhipUsesLeft = 2;
                int[] whipsUsedSoFar = new int[additionalWhipUsesLeft];
                for (var i = 0; i <= 9; i++) { // Please find a better way to iterate through hotbar items
                    Item hotbarItem = player.inventory[i];
                    if (WhipFrenzyPlayer.IsEligibleWhip(hotbarItem) && !whipsUsedSoFar.Contains(hotbarItem.type) && hotbarItem.type != item.type) {
                        Console.WriteLine("Additional Whip Found! " + hotbarItem.AffixName());
                        whipsUsedSoFar[whipsUsedSoFar.Length - additionalWhipUsesLeft] = hotbarItem.type;
                        additionalWhipUsesLeft--;
                        Vector2 shootVel = Main.MouseWorld - player.Center;
                        shootVel.Normalize();
                        shootVel *= 0.5f;
                        int projectile = Projectile.NewProjectile(
                            player.GetSource_ItemUse(hotbarItem), 
                            player.Center,
                            shootVel,
                            hotbarItem.shoot, 
                            (int)player.GetDamage(hotbarItem.DamageType).ApplyTo(hotbarItem.damage),
                            player.GetKnockback(hotbarItem.DamageType).ApplyTo(hotbarItem.knockBack),
                            player.whoAmI,
                            -100f
                            );
                        if (additionalWhipUsesLeft <= 0) {
                            break;
                        }
                    }
                }
                
            }
            
            return true;
        }

        // I don't like adding new keys, but I might have to add one ;_;
        public override bool AltFunctionUse(Item item, Player player) {
            return true;
        }

    }
}
