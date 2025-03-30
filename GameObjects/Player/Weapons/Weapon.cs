using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    public abstract class Weapon : GameObject
    {
        protected Texture2D _texture;
        protected Type _ammo;
        protected Vector2 _position;
        protected float _angle;

        protected string _textureName;

        protected Vector2 _target;

        public Weapon(Vector2 position, Type ammo, string textureName)
        {
            _position = position;
            _ammo = ammo;
            _textureName = textureName;
        }

        public override void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_textureName);
            base.Load(content);
        }

        public void Update(GameTime gameTime, Vector2 position, Vector2 target)
        {
            _position = position;
            _target = target;
            _angle = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(position, _target));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
                if (_texture != null)
                {
                    Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
                    spriteBatch.Draw(_texture, _position, null, Color.White, _angle, origin, 1.0f, SpriteEffects.None, 0);
                }
        }

        public virtual void Shoot(Vector2 target)
        {
            Vector2 aimDirection = LinePieceCollider.GetDirection(_position, target);
            // Console.WriteLine($"Shoot: {target}, {aimDirection}");
            GameObject newObject = (GameObject)Activator.CreateInstance(_ammo, _position, target, aimDirection);
            GameManager.GetGameManager().AddGameObject(newObject);
        }
    }
}
