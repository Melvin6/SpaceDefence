using System;
using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class Ship : GameObject
    {
        private Texture2D ship_body;
        private Texture2D base_turret;
        private Texture2D laser_turret;
        private float buffTimer = 10;
        private float buffDuration = 10f;
        private RectangleCollider _rectangleCollider;
        private Point target;
        private Point position;
        private Point lastPosition;
        private Vector2 velocity = Vector2.Zero;
        private float acceleration = 2f;
        private float maxSpeed = 10f;
        private float rotation = 0f;

        /// <summary>
        /// The player character
        /// </summary>
        /// <param name="Position">The ship's starting position</param>
        public Ship(Point Position)
        {
            position = Position;
            _rectangleCollider = new RectangleCollider(new Rectangle(Position, Point.Zero));
            SetCollider(_rectangleCollider);
        }

        public override void Load(ContentManager content)
        {
            // Ship sprites from: https://zintoki.itch.io/space-breaker
            ship_body = content.Load<Texture2D>("ship_body");
            base_turret = content.Load<Texture2D>("base_turret");
            laser_turret = content.Load<Texture2D>("laser_turret");
            _rectangleCollider.shape.Size = ship_body.Bounds.Size;
            _rectangleCollider.shape.Location -= new Point(ship_body.Width/2, ship_body.Height/2);
            base.Load(content);
        }



        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            target = inputManager.CurrentMouseState.Position;
            if(inputManager.LeftMousePress())
            {

                Vector2 aimDirection = LinePieceCollider.GetDirection(GetPosition().Center, target);
                Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + aimDirection * base_turret.Height / 2f;
                if (buffTimer <= 0)
                {
                    GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, aimDirection, 150));
                }
                else
                {
                    GameManager.GetGameManager().AddGameObject(new Laser(new LinePieceCollider(turretExit, target.ToVector2()),400));
                }
            }

            // movement:
            movement(inputManager);
        }

        private void movement(InputManager inputManager)
        {
            Vector2 inputDirection = Vector2.Zero;
            if (inputManager.MoveUp()) inputDirection.Y -= 1;
            if (inputManager.MoveDown()) inputDirection.Y += 1;
            if (inputManager.MoveLeft()) inputDirection.X -= 1;
            if (inputManager.MoveRight()) inputDirection.X += 1;

            if (inputDirection != Vector2.Zero)
            {
                inputDirection = Vector2.Normalize(inputDirection);
                velocity += inputDirection * acceleration;
            }
            else
            {
                velocity = Vector2.Zero;
            }

            if (velocity.Length() >= maxSpeed)
            {
                velocity = Vector2.Normalize(velocity) * maxSpeed;
            }

            if (velocity != Vector2.Zero) {
                Vector2 newPosition = new Vector2(position.X, position.Y) + velocity;
                lastPosition = position;
                position = newPosition.ToPoint();
            }

            rotation = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(lastPosition,  position));
            wrap();
            UpdateCollider();
        }

        private void wrap()
        {
            int screenWidth = 1280;
            int screenHeight = 720;

            if (position.X < 0)
            {
                position.X = screenWidth;
            }
            else if (position.X > screenWidth)
            {
                position.X = 0;
            }

            if (position.Y < 0)
            {
                position.Y = screenHeight;
            }
            else if (position.Y > screenHeight)
            {
                position.Y = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (buffTimer > 0)
                buffTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Alien)
            {
                GameManager gm = GameManager.GetGameManager();
                gm.GameOver();
            }
            base.OnCollision(other);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship_body, _rectangleCollider.shape, Color.White);
            spriteBatch.Draw(
                ship_body,  
                new Vector2(_rectangleCollider.shape.Center.X, _rectangleCollider.shape.Center.Y),
                null,
                Color.White, 
                rotation,
                new Vector2(ship_body.Width / 2f, ship_body.Height / 2f),
                1.0f,
                SpriteEffects.None, 
                0f                    
            );

            float aimAngle = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(GetPosition().Center, target));
            if (buffTimer <= 0)
            {
                Rectangle turretLocation = base_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(base_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(laser_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            base.Draw(gameTime, spriteBatch);
        }

        private void UpdateCollider()
        {
            Point center = new Point(position.X + ship_body.Width / 2, position.Y + ship_body.Height / 2);

            int newWidth = _rectangleCollider.shape.Width;
            int newHeight = _rectangleCollider.shape.Height;

            _rectangleCollider = new RectangleCollider(new Rectangle(
                center.X - newWidth / 2, 
                center.Y - newHeight / 2, 
                newWidth, 
                newHeight
            ));

            SetCollider(_rectangleCollider);
        }


        public void Buff()
        {
            buffTimer = buffDuration;
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }
    }
}
