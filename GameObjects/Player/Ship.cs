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
        private Animation explosion;
        public Camera Camera { get; private set; }
        public Planet cargo {get; private set;}
        private Weapon _weapon;

        /// <summary>
        /// The player character
        /// </summary>
        /// <param name="Position">The ship's starting position</param>
        public Ship(Point Position)
        {
            position = Position;
            _rectangleCollider = new RectangleCollider(new Rectangle(Position, Point.Zero));
            SetCollider(_rectangleCollider);
            explosion = new Explosion1(Vector2.Zero);

            _weapon = new BulletWeapon(position.ToVector2());
        }

        public override void Load(ContentManager content)
        {
            // Ship sprites from: https://zintoki.itch.io/space-breaker
            ship_body = content.Load<Texture2D>("ship_body");
            base_turret = content.Load<Texture2D>("base_turret");
            laser_turret = content.Load<Texture2D>("laser_turret");
            _rectangleCollider.shape.Size = ship_body.Bounds.Size;
            _rectangleCollider.shape.Location -= new Point(ship_body.Width/2, ship_body.Height/2);
            Camera = new Camera(GameManager.GetGameManager().Game.GraphicsDevice.Viewport);
            explosion.Load(content);
            _weapon.Load(content);
            base.Load(content);
        }



        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            GameManager gm = GameManager.GetGameManager();
            // target = inputManager.GetMouseWorldPosition(Camera.Transform).ToPoint();
            if(gm.GetState() == GameState.Running){
                if(inputManager.LeftMousePress())
                {
                    Console.WriteLine($"input: {target.ToVector2()}");
                    _weapon.Shoot(target.ToVector2());
                    // Vector2 aimDirection = LinePieceCollider.GetDirection(GetPosition().Center, target);
                    // Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + aimDirection * base_turret.Height / 2f;
                    // if (buffTimer <= 0)
                    // {
                    //     GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, aimDirection));
                    // }
                    // else
                    // {
                    //     GameManager.GetGameManager().AddGameObject(new Laser(turretExit, target.ToVector2()));
                    // }
                }

                // movement:
                movement(inputManager);
            }
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
                Rectangle bounds = GameManager.GetGameManager().level.Bounds;
                Vector2 newPosition = new Vector2(Math.Clamp(position.X, bounds.Left, bounds.Right), Math.Clamp(position.Y, bounds.Left, bounds.Right)) + velocity;
                lastPosition = position;
                position = newPosition.ToPoint();
            }

            rotation = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(lastPosition,  position));
            wrap();
            UpdateCollider();
        }

        private void wrap()
        {
            int left = GameManager.GetGameManager().level.Bounds.Left;
            int right = GameManager.GetGameManager().level.Bounds.Right;
            int top = GameManager.GetGameManager().level.Bounds.Top;
            int bottom = GameManager.GetGameManager().level.Bounds.Bottom;

            if (position.X < left)
                position.X = right;
            else if (position.X > right)
                position.X = left;

            if (position.Y < top)
                position.Y = bottom;
            else if (position.Y > bottom)
                position.Y = top;
        }

        public override void Update(GameTime gameTime)
        {
            explosion.Update(gameTime);
            GameManager gm = GameManager.GetGameManager();
            target = GameManager.GetGameManager().InputManager.GetMouseWorldPosition(Camera.Transform).ToPoint();
            
            if(gm.GetState() == GameState.Running){
            if (buffTimer > 0)
            {
                buffTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (buffTimer <= 0 && _weapon is not BulletWeapon)
            {
                _weapon = new BulletWeapon(position.ToVector2());
                _weapon.Load(GameManager.GetGameManager()._content);
            }

                base.Update(gameTime);
            }
            Camera.Update(new Vector2 (GetPosition().Center.X, GetPosition().Center.Y));
            _weapon.Update(gameTime, _rectangleCollider.shape.Center.ToVector2(), GameManager.GetGameManager().InputManager.GetMouseWorldPosition(Camera.Transform));
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Alien || other is Asteroid)
            {
                explosion.updatePosition(_rectangleCollider.GetBoundingBox().Center.ToVector2());
                explosion.Play();

                GameManager gm = GameManager.GetGameManager();
                gm.GameOver();
            }
            else if (other is not Ammo) {

                position = lastPosition;
                velocity = Vector2.Zero;
            }
            base.OnCollision(other);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            explosion.Draw(spriteBatch);
            spriteBatch.Draw(
                ship_body,  
                new Vector2(_rectangleCollider.shape.Center.X, _rectangleCollider.shape.Center.Y),
                null,
                Color.White, 
                rotation,
                new Vector2(ship_body.Width / 2f, ship_body.Height / 2f),
                1.0f,
                SpriteEffects.None, 
                1f                    
            );

            _weapon.Draw(gameTime, spriteBatch);
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


        public void Buff(Weapon type)
        {
            buffTimer = buffDuration;
            _weapon = type;
            _weapon.Load(GameManager.GetGameManager()._content);
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }

        public Point GetPoint()
        {
            return position;
        }

        public void pickUpCargo(Planet planet)
        {
            if (cargo != null && cargo.GetType() != planet.GetType())
            {
                cargo = null;
                GameManager.GetGameManager().AddScore(1);
            }
            else
                cargo = planet;

        }

        public void ResetCargo() {cargo = null;}
    }
}
