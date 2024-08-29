using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerSaveSystem : ModSystem {

        public static Item[] reforgeHammerStorage = new Item[1] { new Item(0) };
        private const string HAMMER_SLOT_TAG = "elytrafae_ACTIVE_TINKERER_HAMMER";

        public override void SaveWorldData(TagCompound tag) {
            tag.Add(HAMMER_SLOT_TAG, reforgeHammerStorage[0]);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.TryGet(HAMMER_SLOT_TAG, out Item hammer)) {
                reforgeHammerStorage[0] = hammer;
            }
        }

        public static Item GetSelectedHammer() {
            return reforgeHammerStorage[0];
        }

        public static void SetSelectedHammer(Item hammer) {
            if (hammer == null) {
                return;
            }
            reforgeHammerStorage[0] = hammer;
        }

    }
}
