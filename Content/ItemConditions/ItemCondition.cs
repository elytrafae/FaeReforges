using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System;

namespace FaeReforges.Content.ItemConditions {
    public record class ItemCondition(LocalizedText Text, Func<Item, bool> Predicate) {
        public ItemCondition(string LocalizationKey, Func<Item, bool> Predicate) : this(Language.GetOrRegister(LocalizationKey), Predicate) { }
        public ItemCondition(Mod mod, string name, Func<Item, bool> Predicate) : this(mod.GetLocalization($"ItemConditions.{name}"), Predicate) { }
        public bool IsMet(Item item) => Predicate(item);

        public static LocalizedText QuickName(string name) => Language.GetOrRegister($"Mods.{nameof(FaeReforges)}.ItemConditions.{name}");

        public static readonly ItemCondition Any = new(QuickName(nameof(Any)), (item) => true);
        public static readonly ItemCondition IsWeapon = new(QuickName(nameof(IsWeapon)), (Item item) => item.damage > 0 && item.ammo == AmmoID.None);
        public static readonly ItemCondition IsMeleeWeapon = new(QuickName(nameof(IsMeleeWeapon)), (Item item) => item.DamageType.CountsAsClass(DamageClass.Melee) && IsWeapon.IsMet(item));
        public static readonly ItemCondition IsWhipWeapon = new(QuickName(nameof(IsWhipWeapon)), (Item item) => item.DamageType.CountsAsClass(DamageClass.SummonMeleeSpeed) && IsWeapon.IsMet(item));
        public static readonly ItemCondition IsRangedWeapon = new(QuickName(nameof(IsRangedWeapon)), (Item item) => item.DamageType.CountsAsClass(DamageClass.Ranged) && IsWeapon.IsMet(item));
        public static readonly ItemCondition IsMagicWeapon = new(QuickName(nameof(IsMagicWeapon)), (Item item) => item.DamageType.CountsAsClass(DamageClass.Magic) && IsWeapon.IsMet(item));
        public static readonly ItemCondition IsSummonWeapon = new(QuickName(nameof(IsSummonWeapon)), (Item item) => item.DamageType.CountsAsClass(DamageClass.Summon) && !IsWhipWeapon.IsMet(item) && IsWeapon.IsMet(item));
        public static readonly ItemCondition IsAccessory = new(QuickName(nameof(IsAccessory)), (Item item) => item.accessory);

        // Logical
        public static ItemCondition GrammaticalAnd(ItemCondition cond1, ItemCondition cond2) => new(QuickName(nameof(GrammaticalAnd)).WithFormatArgs(cond1.Text, cond2.Text), (item) => cond1.IsMet(item) || cond2.IsMet(item));
        public static ItemCondition GrammaticalAnd3(ItemCondition cond1, ItemCondition cond2, ItemCondition cond3) => new(QuickName(nameof(GrammaticalAnd3)).WithFormatArgs(cond1.Text, cond2.Text, cond3.Text), (item) => cond1.IsMet(item) || cond2.IsMet(item) || cond3.IsMet(item));
        public static ItemCondition Not(ItemCondition cond) => new(QuickName(nameof(Not)).WithFormatArgs(cond.Text), (item) => !cond.IsMet(item));
        public static ItemCondition ThisButNotThis(ItemCondition trueCond, ItemCondition falseCond) => new(QuickName(nameof(ThisButNotThis)).WithFormatArgs(trueCond.Text, falseCond.Text), (item) => trueCond.IsMet(item) && !falseCond.IsMet(item));

    }
}
