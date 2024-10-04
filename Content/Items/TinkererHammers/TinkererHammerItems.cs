using FaeReforges.Systems.LootTableModification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaeReforges.Content.Items.TinkererHammers {

    // Prehardmode

    /*
    public class StoneTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Gray;
        public override int Value => 5;

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 30)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class CopperTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 1, copper: 50);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class TinTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 1, copper: 50);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TinBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class IronTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 3);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class LeadTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 3);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class SilverTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 6);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class TungstenTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 6);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class GoldenTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 12);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class PlatinumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 12);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class DemoniteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 30);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 15)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.Ebonwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class CrimtaneTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 30);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 15)
                .AddIngredient(ItemID.TissueSample, 5)
                .AddIngredient(ItemID.Shadewood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

        
    public class HellstoneTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 40);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddIngredient(ItemID.AshWood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }
    */

    public class MeteoriteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.White;
        public override int Value => Terraria.Item.buyPrice(silver: 30);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    // Hardmode

    public class CobaltTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 21);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class PalladiumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 21);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.PalladiumBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class MythrilTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 44);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class OrichalcumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 44);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.OrichalcumBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class AdamantiteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 68);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class TitaniumTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Orange;
        public override int Value => Terraria.Item.buyPrice(silver: 68);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 15)
                .AddIngredient(ItemID.Pearlwood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class HallowedTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int Value => Terraria.Item.buyPrice(silver: 80);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class FrostTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int Value => Terraria.Item.buyPrice(silver: 80);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.FrostCore, 1)
                .AddIngredient(ItemID.IceBlock, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class ForbiddenTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.LightRed;
        public override int Value => Terraria.Item.buyPrice(silver: 80);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                .AddIngredient(ItemID.FossilOre, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class ChlorophyteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(silver: 90);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class VenomiteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(silver: 90);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddIngredient(ItemID.VialofVenom, 5)
                .AddIngredient(ItemID.SpiderFang, 10)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class SpectreTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(silver: 90);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 15)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class ShroomiteTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Lime;
        public override int Value => Terraria.Item.buyPrice(silver: 90);

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.ShroomiteBar, 15)
                .AddIngredient(ItemID.RichMahogany, 15)
                .AddIngredient(ItemID.Rope, 5)
                .AddTile<Content.Tiles.TinkererAnvil>()
                .Register();
        }
    }

    public class SolarTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Red;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 20);

        public override void AddRecipes() {
            CreateRecipe()
                .AddCondition(MoonLordBag.NEVER_CONDITION)
                .AddCustomShimmerResult(ModContent.ItemType<VortexTinkererHammer>(), 1)
                .Register();
        }
    }

    public class VortexTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Red;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 20);

        public override void AddRecipes() {
            CreateRecipe()
                .AddCondition(MoonLordBag.NEVER_CONDITION)
                .AddCustomShimmerResult(ModContent.ItemType<NebulaTinkererHammer>(), 1)
                .Register();
        }
    }

    public class NebulaTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Red;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 20);

        public override void AddRecipes() {
            CreateRecipe()
                .AddCondition(MoonLordBag.NEVER_CONDITION)
                .AddCustomShimmerResult(ModContent.ItemType<StardustTinkererHammer>(), 1)
                .Register();
        }
    }

    public class StardustTinkererHammer : SimpleTinkererHammerItem {
        public override int Rarity => ItemRarityID.Red;
        public override int Value => Terraria.Item.buyPrice(gold: 1, silver: 20);

        public override void AddRecipes() {
            CreateRecipe()
                .AddCondition(MoonLordBag.NEVER_CONDITION)
                .AddCustomShimmerResult(ModContent.ItemType<SolarTinkererHammer>(), 1)
                .Register();
        }
    }

    // TODO: Continue with other mods after initial release


}
