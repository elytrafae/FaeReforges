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

        public VanillaReforgeOverrideData(float damage, float knockback, float speed, float size, float velocity, float mana, int crit) {
            this.damage = damage;
            this.knockback = knockback;
            this.speed = speed;
            this.size = size;
            this.velocity = velocity;
            this.mana = mana;
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
