using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class Score : HudObject
    {
        public Score(Vector2 position) : base(position, "Score: 0", Color.White)
        {
        }

        public override string Text => $"Score: {GameManager.GetGameManager().score.ToString()}";

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
