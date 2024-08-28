using FaeReforges.Content;
using FaeReforges.Content.HammerTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FaeReforges.Systems.ReforgeHammers {
    public class ReforgeHammerSaveSystem : ModSystem {

        private static List<AbstractHammerType> unlockedHammers = new List<AbstractHammerType>();
        private const string UNLOCKED_HAMMERS_TAG = "elytrafae_UNLOCKED_HAMMERS";

        private static AbstractHammerType selectedHammer = null;
        private const string SELECTED_HAMMER_TAG = "elytrafae_SELECTED_HAMMER";

        public override void SaveWorldData(TagCompound tag) {
            tag.Add(UNLOCKED_HAMMERS_TAG, GenerateFullNameList());
            tag.Add(SELECTED_HAMMER_TAG, GetSelectedHammer().FullName);
        }

        public override void LoadWorldData(TagCompound tag) {
            if (tag.TryGet(UNLOCKED_HAMMERS_TAG, out List<string> names)) {
                PopulateTypeList(names);
            }
            AbstractHammerType stoneHammer = ModContent.GetContent<StoneHammer>().First();
            UnlockHammer(stoneHammer); // Default Hammer!

            if (tag.TryGet(SELECTED_HAMMER_TAG, out string selectedName)) {
                selectedHammer = GetHammerTypeFromName(selectedName);
            }
            // If the hammer name was invalid, or was not set at all, get default
            if (selectedHammer == null) {
                selectedHammer = stoneHammer;
            }
        }

        public static bool IsHammerUnlocked(AbstractHammerType hammer) { 
            return unlockedHammers.Contains(hammer);
        }

        public static void UnlockHammer(AbstractHammerType hammer) {
            if (!unlockedHammers.Contains(hammer)) { 
                unlockedHammers.Add(hammer);
            }
        }

        public static AbstractHammerType GetSelectedHammer() {
            if (selectedHammer == null) {
                return ModContent.GetContent<StoneHammer>().First();
            }
            return selectedHammer;
        }

        public static void SetSelectedHammer(AbstractHammerType hammer) {
            if (hammer == null) {
                return;
            }
            selectedHammer = hammer;
        }

        public static IEnumerable<AbstractHammerType> GetAllUnlockedHammers() {
            return unlockedHammers;
        }

        private List<string> GenerateFullNameList() {
            List<string> names = new();
            foreach (AbstractHammerType type in unlockedHammers) { 
                names.Add(type.FullName);
            }
            return names;
        }

        private void PopulateTypeList(List<string> names) {
            IEnumerable<AbstractHammerType> types = ModContent.GetContent<AbstractHammerType>(); // This is for optimization reasons
            unlockedHammers.Clear();
            foreach (string name in names) {
                AbstractHammerType type = GetHammerTypeFromName(types, name);
                if (type != null) {
                    unlockedHammers.Add(type);
                }
            }
        }

        public static AbstractHammerType GetHammerTypeFromName(string hammerName) {
            return GetHammerTypeFromName(ModContent.GetContent<AbstractHammerType>(), hammerName);
        }

        public static AbstractHammerType GetHammerTypeFromName(IEnumerable<AbstractHammerType> enumerable, string hammerName) {
            foreach (var type in enumerable) {
                if (type.FullName == hammerName) {
                    return type;
                }
            }
            return null;
        }

    }
}
