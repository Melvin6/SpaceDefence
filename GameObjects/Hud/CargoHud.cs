using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class CargoHud : HudObject
    {
        public CargoHud(Vector2 position) : base(position, "No Cargo", Color.White)
        {
        }

        public override string Text => GameManager.GetGameManager().Player.cargo != null ? $"Cargo: {GameManager.GetGameManager().Player.cargo.Name}" : "No Cargo";

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
