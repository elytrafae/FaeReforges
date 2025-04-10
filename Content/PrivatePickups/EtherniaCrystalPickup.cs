using FaeReforges.Content.Items.TinkererHammers.Tier3;
using FaeReforges.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FaeReforges.Content.Buffs;

namespace FaeReforges.Content.PrivatePickups {
    public class EtherniaCrystalPickup : SimpleTinkererHammerPrivatePickup {

        public override void SimpleSetDefaults() {
            texture = MiscSpritesSystem.EtherniaCrystalPickup;
            reactToPlayer = true;
            dustType = DustID.PurpleCrystalShard;
            killSound = SoundID.DD2_DefenseTowerSpawn.WithPitchOffset(-2f).WithVolumeScale(0.6f);
            hammerType = ModContent.ItemType<MonkTinkererHammer>();
            lifetime = 460;
        }

        public override bool SimpleOnCollide(Player player) {
            player.AddBuff(ModContent.BuffType<General>(), 10 * 60, false);
            return true;
        }

    }
}
