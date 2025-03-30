using System;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    public abstract class Planet : GameObject
    {
        public string Name {get; private set;}
        private CircleCollider _circleCollider;
        private float playerClearance = 500;
        private Animation _planet;
        private Vector2 _position;
        private bool cargo;

        public Planet(Animation planet, Vector2 position, string name) 
        {
            _position = position;
            _planet = planet;
            Name = name;
            cargo = true;
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _planet.Load(content);
            _circleCollider = new CircleCollider(_position, (_planet.animation.myTexture.Height * _planet.animation.Scale) /2 * 0.7f);
            SetCollider(_circleCollider);
            _planet.updatePosition(_circleCollider.Center - new Vector2(_planet.animation.myTexture.Height * _planet.animation.Scale  / 2, _planet.animation.myTexture.Height * _planet.animation.Scale / 2));
            _planet.Play();
        }

        public override void Update(GameTime gameTime)
        {
            _planet.Update(gameTime);
            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Ship ship && cargo)
            {
                ship.pickUpCargo(this);
                cargo = false; 
            }
            else if (other is Ship ship1 && !cargo) 
            {
                ship1.pickUpCargo(this);
                cargo = true;
            }
            base.OnCollision(other);
        }

        public void RandomMove()
        {
            GameManager gm = GameManager.GetGameManager();
            _circleCollider.Center = Vector2.Zero;

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
                _circleCollider.Center = gm.RandomScreenLocation();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _planet.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }


    }
}
