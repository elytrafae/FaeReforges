using FaeLibrary.API.ItemConditions;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using FaeReforges.Systems.ReforgeHammers;

namespace FaeReforges.Content.Items.TinkererHammers.Tier4 {
    public class PrismaticTinkererHammer : SimpleTinkererHammerItem {

        public const float POWER = 0.03f;

        public override int Rarity => ItemRarityID.Yellow;
        public override ItemCondition ReforgeableCondition => ItemCondition.IsAccessory;
        public override int HammerTier => 4;

        public override string GetAccessoryEffectText(Item item) {
            return AccessoryEffectText.Format(POWER * 100);
        }

        public override void SetHammerDefaults() {
            
        }

        public override void HammerOnUpdateAccessory(Item item, Player player, int count, bool hideVisual) {
            if (player.wingTime < player.wingTimeMax || player.rocketTime < player.rocketTimeMax) {
                player.moveSpeed += POWER; // This variable also affects acceleration. Don't ask.
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, null, ReforgeHammerSavePlayer.GetLighterRainbowColor(), 0, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Rectangle frame = Item.getRect();
            Vector2 vector = frame.Size() / 2f;
            Vector2 vector2 = new((float)(Item.width / 2) - vector.X, (float)(Item.height - frame.Height));
            Vector2 drawPos = Item.position - Main.screenPosition + vector + vector2;
            //Vector2 drawPos = Item.position - Main.screenPosition + new Vector2(0, 32);
            Color newColor = lightColor.MultiplyRGBA(ReforgeHammerSavePlayer.GetLighterRainbowColor());
            //spriteBatch.Draw(WritingTexture.Value, drawPos, null, newColor, rotation, new Vector2(0, 32), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(TextureAssets.Item[Type].Value, drawPos, null, newColor, rotation, vector, scale, SpriteEffects.None, 0f);
            return false;
        }



    }
}
