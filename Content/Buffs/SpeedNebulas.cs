using FaeLibrary.API.ClassExtensions;
using FaeReforges.Systems.VanillaReforges;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Buffs {
    public abstract class SpeedNebulaTemplate : ModBuff {

        public override void SetStaticDefaults() {
            Main.debuff[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public abstract int Power { get; }

        // We are NOT giving this to NPC. It would make no sense

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<VanillaReforgePlayer>().accessoryMovement += (15 * Power);
            player.GetWingTimeStat() += 0.15f * Power;
        }

    }

    public class SpeedNebula1 : SpeedNebulaTemplate {
        public override int Power => 1;
    }

    public class SpeedNebula2 : SpeedNebulaTemplate {
        public override int Power => 2;
    }

    public class SpeedNebula3 : SpeedNebulaTemplate {  
        public override int Power => 3;
    }

}
