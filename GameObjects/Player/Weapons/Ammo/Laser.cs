using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class Laser : Ammo
    {

        public Laser(Vector2 location, Vector2 target, Vector2 direction) : base (new LinePieceCollider(location,  target), Vector2.Zero, 0.25f)
        // public Laser(LinePieceCollider linePiece,) : base (linePiece, Vector2.Zero, 0.25f)
        {
            _collider = new LinePieceCollider(location,  target);
            Console.WriteLine($"Laser: {location}, {target}");
            SetCollider(_collider);

            if (_collider is LinePieceCollider linePiece)
            {
                linePiece.Length = 400;
            }
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("Laser");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_lifespan < 0)
                GameManager.GetGameManager().RemoveGameObject(this);
            _lifespan -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_collider is LinePieceCollider linePiece)
            {
                Rectangle target = new Rectangle((int)linePiece.Start.X, (int)linePiece.Start.Y, _texture.Width, (int)linePiece.Length);
                spriteBatch.Draw(_texture, target, null,Color.White, linePiece.GetAngle(), new Vector2(_texture.Width/2f,_texture.Height),SpriteEffects.None,1 );
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
