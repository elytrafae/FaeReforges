using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges {
    public class VanillaReforgeOverrideData {

        readonly public float damage;
        readonly public float knockback;
        readonly public float speed;
        readonly public float size;
        readonly public float velocity;
        readonly public float mana;
        readonly public int crit;

        // The speed and mana cost are with a - because it's actually use time/mana cost increase that is being applied
        public VanillaReforgeOverrideData(float damage, float knockback, float speed, float size, float velocity, float mana, int crit) {
            this.damage = 1f + damage;
            this.knockback = 1f + knockback;
            this.speed = 1f - speed;
            this.size = 1f + size;
            this.velocity = 1f + velocity;
            this.mana = 1f - mana;
            this.crit = crit;
        }

        public void ApplyTo(out float dmg, out float kb, out float spd, out float size, out float shtspd, out float mcst, out int crt) {
            dmg = this.damage;
            kb = this.knockback; 
            spd = this.speed;
            size = this.size;
            shtspd = this.velocity;
            mcst = this.mana;
            crt = this.crit;
        }

    }
}
