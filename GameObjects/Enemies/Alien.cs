using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class Alien : GameObject
    {
        private CircleCollider _circleCollider;
        private Texture2D _texture;
        private float playerClearance = 100;
        private static float speedMultiplier = 1.0f;
        private Vector2 velocity;
        private Animation explosion;

        public Alien() 
        {
            explosion = new Explosion1(Vector2.Zero);
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("Alien");
            _circleCollider = new CircleCollider(Vector2.Zero, _texture.Width / 2);
            SetCollider(_circleCollider);
            RandomMove();

            explosion.Load(content);
        }

        public override void Update(GameTime gameTime)
        {
            Movement();
            explosion.Update(gameTime);
            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            explosion.updatePosition(_circleCollider.Center - new Vector2(_texture.Width / 2, _texture.Height / 2));
            explosion.Play();
            RandomMove();
            base.OnCollision(other);
        }

        private void Movement()
        {
            GameManager gm = GameManager.GetGameManager();
            Vector2 playerPos = gm.Player.GetPosition().Center.ToVector2();
            Vector2 direction = playerPos - _circleCollider.Center;

            if (direction.Length() > 0 && gm.GetState() == GameState.Running)
            {
                direction.Normalize();
                velocity = direction * (100f * speedMultiplier) * (float)GameManager.GetGameManager().Game.TargetElapsedTime.TotalSeconds;
                _circleCollider.Center += velocity;
            }
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            _circleCollider.Center = gm.RandomScreenLocation();

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
                _circleCollider.Center = gm.RandomScreenLocation();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
            explosion.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }

        public Vector2 getPosition()
        {
            return _circleCollider.Center;
        }


    }
}
