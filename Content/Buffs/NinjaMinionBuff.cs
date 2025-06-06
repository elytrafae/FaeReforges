using FaeReforges.Content.Projectiles.NinjaMinion;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public class NinjaMinionBuff : ModBuff {

        public override void SetStaticDefaults() {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            int minionType = ModContent.ProjectileType<NinjaMinion>();
            if (player.ownedProjectileCounts[minionType] <= 0 && player.whoAmI == Main.myPlayer) {
                int baseDamage = 60;
                int damage = (int)player.GetDamage(DamageClass.Summon).ApplyTo(baseDamage);
                float baseKB = 1.2f;
                float kb = player.GetKnockback(DamageClass.Summon).ApplyTo(baseKB);
                int projWAI = Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, minionType, damage, kb, player.whoAmI);
                if (projWAI >= 0 && projWAI < Main.maxProjectiles) { 
                    Projectile proj = Main.projectile[projWAI];
                    proj.originalDamage = baseDamage;
                }
            }
        }

    }
}
