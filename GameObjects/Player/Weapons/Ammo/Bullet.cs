using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class Bullet : Ammo
    {
        public float bulletSize = 4;

        public Bullet(Vector2 location, Vector2 target, Vector2 direction) : base (new CircleCollider(location, 4), direction * 150, 0.25f)
        {
            _collider = new CircleCollider(location, bulletSize);
            SetCollider(_collider);
        }

        public override void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Bullet");
            base.Load(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_collider is CircleCollider circle)
            {
                circle.Center += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!GameManager.GetGameManager().level.Bounds.Contains(circle.Center))
                    GameManager.GetGameManager().RemoveGameObject(this);
            }

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Alien || other is Supply || other is Asteroid || other is Planet)
            {
                GameManager.GetGameManager().RemoveGameObject(this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _collider.GetBoundingBox(), Color.Red);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
