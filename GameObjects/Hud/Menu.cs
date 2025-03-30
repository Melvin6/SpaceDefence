using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class Menu : GameObject
    {
        private List<string> _menuItems;
        private int _selectedIndex;
        private SpriteFont _font;
        private Vector2 _position;
        private Color _normalColor = Color.White;
        private Color _selectedColor = Color.Yellow;
        
        public delegate void MenuAction();
        private List<MenuAction> _actions;

        public Menu(List<string> menuItems, List<MenuAction> actions, Vector2 position)
        {
            _menuItems = menuItems;
            _actions = actions;
            _position = position;
            _selectedIndex = 0;
        }

        public override void Load(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Arial");
        }

        public override void HandleInput(InputManager inputManager)
        {
            if (inputManager.IsKeyPress(Keys.Down))
            {
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
            }
            else if (inputManager.IsKeyPress(Keys.Up))
            {
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
            }
            else if (inputManager.IsKeyPress(Keys.Enter))
            {
                _actions[_selectedIndex]?.Invoke();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            Vector2 position = _position;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                Color color = (i == _selectedIndex) ? _selectedColor : _normalColor;
                spriteBatch.DrawString(_font, _menuItems[i], position, color);
                position.Y += 40;
            }
        }
    }
}
