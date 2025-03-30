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
            spriteBatch.Draw(_texture, new Vector2(_rectangleCollider.shape.X, _rectangleCollider.shape.Y), null, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
