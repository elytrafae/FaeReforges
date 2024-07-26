using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Content.Reforges {
    public class SimpleCustomReforgeTemplate : ModPrefix {

        readonly string name;
        readonly PrefixCategory category;
        readonly bool positive;
        readonly float damage;
        readonly float knockback;
        readonly float speed;
        readonly float size;
        readonly float velocity;
        readonly float mana;
        readonly int crit;

        public SimpleCustomReforgeTemplate(string name, PrefixCategory category, bool positive, float damage, float knockback, float speed, float size, float velocity, float mana, int crit) {
            this.name = name;
            this.category = category;
            this.positive = positive;
            this.damage = damage;
            this.knockback = knockback;
            this.speed = speed;
            this.size = size;
            this.velocity = velocity;
            this.mana = mana;
            this.crit = crit;
        }

        public override string Name => name;
        public override PrefixCategory Category => category;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus) {
            damageMult = 1f + damage;
            knockbackMult = 1f + knockback;
            useTimeMult = 1f + speed;
            scaleMult = 1f + size;
            shootSpeedMult = 1f + velocity;
            manaMult = 1f + mana;
            critBonus = crit;
        }

        public override void ModifyValue(ref float valueMult) {
            valueMult = positive ? 1.5f : 0.5f;
        }

    }
}
