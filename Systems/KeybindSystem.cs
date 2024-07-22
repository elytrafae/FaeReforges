using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FaeReforges.Systems
{
    public class KeybindSystem : ModSystem
    {

        public static ModKeybind WhipFrenzyKeybind { get; private set; }

        public override void Load() {
            WhipFrenzyKeybind = KeybindLoader.RegisterKeybind(Mod, "WhipFrenzy", "2");
        }

        public override void Unload() {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            WhipFrenzyKeybind = null;
        }

    }
}
