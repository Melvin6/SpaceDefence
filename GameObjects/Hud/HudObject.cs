using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class HudObject : GameObject
    {
        private SpriteFont _font;
        private Vector2 _position;
        private Color _color = Color.White;
        protected string _text;

        public virtual string Text
        {
            get => _text;
            set => _text = value;
        }

        public HudObject(Vector2 position, string text, Color color)
        {
            _position = position;
            _text = text;
            _color = color;
        }

        public override void Load(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Arial");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Text, _position, _color);
        }
    }
}
