using FaeLibrary.API.ItemConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class BetsyTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Yellow;
        public override int HammerTier => 4;

        public override ItemCondition ReforgeableCondition => ItemCondition.IsWeapon;

        public override void HammerOnWeaponDealDamageNpc(int item, Player attacker, NPC victim, NPC.HitInfo hitInfo, int damageDone) {
            if (hitInfo.Crit) {
                victim.AddBuff(BuffID.BetsysCurse, 60);
            }
        }

        // Players cannot take crits
    }
}
