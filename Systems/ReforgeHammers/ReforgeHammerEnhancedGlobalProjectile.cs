using FaeLibrary.API;
using FaeReforges.Content;
using FaeReforges.Content.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace FaeReforges.Systems.ReforgeHammers {
    internal class ReforgeHammerEnhancedGlobalProjectile : GlobalProjectile, IFaeGlobalProjectile {

        private int hammerItemId = ItemID.None;
        private int createdByItemId = ItemID.None;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            Item createdBy = null;
            if (source is EntitySource_ItemUse itemUseSource) {
                createdBy = itemUseSource.Item;
            } else if (source is IEntitySource_WithStatsFromItem statsSource) {
                createdBy = statsSource.Item;
            } else if (source is EntitySource_Parent parentSource && parentSource.Entity is Projectile parentProj) {
                ReforgeHammerEnhancedGlobalProjectile modProj = parentProj.GetGlobalProjectile<ReforgeHammerEnhancedGlobalProjectile>();
                createdByItemId = modProj.createdByItemId;
                hammerItemId = modProj.hammerItemId;
            }
            if (createdBy != null) {
                createdByItemId = createdBy.type;
                // We assume all hammer effects directly related to projectiles are weapon effects, and will be like this forever.
                AbstractTinkererHammer hammer = createdBy.GetGlobalItem<ReforgeHammerEnhancedGlobalItem>().GetHammer(createdBy, Enums.HammerEffectContext.WEAPON);
                hammerItemId = hammer == null ? ItemID.None : hammer.Type;
            }

            Hammer?.HammerOnCreateProjectile(createdByItemId, projectile, source);
        }

        private AbstractTinkererHammer? Hammer {
            get {
                Item hammerItem = ContentSamples.ItemsByType[hammerItemId];
                if (hammerItem.ModItem != null && hammerItem.ModItem is AbstractTinkererHammer hammer) {
                    return hammer;
                }
                return null;
            }
        }

        // The following is for melee hits only. For projectiles, see ReforgeHammerEnhancedGlobalProjectile
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
            Hammer?.HammerChangeWeaponDealDamageNpc(createdByItemId, Main.player[projectile.owner], target, ref modifiers, projectile.DamageType);
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
            Hammer?.HammerChangeWeaponDealDamagePvp(createdByItemId, Main.player[projectile.owner], target, ref modifiers, projectile.DamageType);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            Hammer?.HammerOnWeaponDealDamageNpc(createdByItemId, Main.player[projectile.owner], target, hit, damageDone, projectile.DamageType);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            Hammer?.HammerOnWeaponDealDamagePvp(createdByItemId, Main.player[projectile.owner], target, info, projectile.DamageType);
        }

        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight) {
            if (!projectile.noEnchantmentVisuals) {
                Hammer?.HammerEnchantmentVisuals(Main.player[projectile.owner], createdByItemId, boxPosition, boxWidth, boxHeight);
            }
        }

        void IFaeGlobalProjectile.ModifyContinuouslyUpdatingDamage(Projectile projectile, Player owner, ref StatModifier damage) {
            Hammer?.HammerModifyWeaponDamage(projectile, owner, ref damage);
        }

        void IFaeGlobalProjectile.ModifyContinuouslyUpdatingCritChance(Projectile projectile, Player owner, ref float crit) {
            Hammer?.HammerModifyWeaponCrit(projectile, owner, ref crit);
        }

        public int GetHammerItemTypeOrNone() {
            return hammerItemId;
        }

        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) {
            // We only care if the hammer is defined!
            if (hammerItemId > ItemID.None) {
                binaryWriter.Write(true);
                binaryWriter.Write(hammerItemId);
                binaryWriter.Write(createdByItemId);
            } else {
                binaryWriter.Write(false);
            }
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) {
            if (binaryReader.ReadBoolean()) {
                hammerItemId = binaryReader.ReadInt32();
                createdByItemId = binaryReader.ReadInt32();
            }
        }

    }
}
