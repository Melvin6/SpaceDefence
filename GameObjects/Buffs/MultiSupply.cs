using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class MultiSupply : Buff
    {
        public MultiSupply() : base (new MultiWeapon(Vector2.Zero), "Crate")
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangleCollider.shape.Center.ToVector2(), null, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
