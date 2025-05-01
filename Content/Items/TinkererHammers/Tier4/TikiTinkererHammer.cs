using FaeLibrary.API.ItemConditions;
using FaeReforges.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class TikiTinkererHammer : SimpleTinkererHammerItem {

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.ThisButNotThis(ItemCondition.IsWeapon, ItemCondition.GrammaticalAnd(ItemCondition.IsMinionWeapon, ItemCondition.IsSentryWeapon));
        public override int HammerTier => 4;

        public override void HammerOnUpdateWeaponHeld(Item item, Player player) {
            int projType = ModContent.ProjectileType<TikiMask>();
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0) {
                DamageClass dmgType = item.DamageType;
                int baseDmg = 50;
                float baseKB = 0;
                Projectile proj = Projectile.NewProjectileDirect(player.GetSource_Misc("Tiki Hammer On Hold Effect"), player.Center, Vector2.Zero, projType, (int)player.GetDamage(dmgType).ApplyTo(baseDmg), player.GetKnockback(dmgType).ApplyTo(baseKB), player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
                if (proj != null && proj.active) { 
                    proj.DamageType = dmgType;
                    proj.originalDamage = baseDmg;
                    proj.OriginalArmorPenetration = 10;
                    proj.OriginalCritChance = 0;
                    ((TikiMask)proj.ModProjectile).summonedByItem = item;
                }
            }
        }

    }
}
