using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FaeReforges.Systems.VanillaReforges {
    public class VanillaReforgePlayer : ModPlayer {

        //public static readonly float[] ACCESSORY_VALUES_PER_POWER = [1f, 1.1025f, 1.21f, 1.3225f, 1.44f];
        public static readonly float[] ACCESSORY_VALUES_PER_POWER = [1f, 1.05f, 1.1f, 1.15f, 1.2f];

        public int manaPercentageRegenPerSecond = 0;
        public float remainingManaRegen = 0; // NOT A STAT! DO NOT RESET EVERY FRAME!
        public int accessoryMovement = 0;
        public int shootVelocityPoints = 0;
        public int ammoSavePoints = 0;

        public override void ResetEffects() {
            manaPercentageRegenPerSecond = 0;
            accessoryMovement = 0;
            shootVelocityPoints = 0;
            ammoSavePoints = 0;
        }

        public override void PostUpdate() {
            // In order for "On Reached Max Mana" events to work properly, we will only regen up to 1 less than Max Mana
            if (manaPercentageRegenPerSecond > 0 && Player.manaRegenDelay <= 0 && Player.statMana < (Player.statManaMax2 - 1)) {
                float manaRegeneratedThisTick = manaPercentageRegenPerSecond * Player.statManaMax2 / 6000f; // 6000 = 60 ticks per second * 100 percent
                int actualManaRegeneratedThisTick = (int)manaRegeneratedThisTick;
                remainingManaRegen += (manaRegeneratedThisTick - actualManaRegeneratedThisTick);
                int bonusManaRegeneratedThisTick = (int)remainingManaRegen;
                remainingManaRegen -= bonusManaRegeneratedThisTick;
                Player.statMana = Math.Min(Player.statManaMax2-1, Player.statMana + actualManaRegeneratedThisTick + bonusManaRegeneratedThisTick);
            }
            
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            velocity *= 1f + (shootVelocityPoints / 100f);
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo) {
            return Main.rand.Next(100) < ammoSavePoints;
        }

        public override void PostUpdateMiscEffects() {
            //Player.moveSpeed *= 1f + accessoryMovement / 100f;
        }

        public override void PostUpdateRunSpeeds() {
            float mov = 1f + accessoryMovement / 100f;
            Player.accRunSpeed *= mov;
            Player.maxRunSpeed *= mov;
        }

    }
}
