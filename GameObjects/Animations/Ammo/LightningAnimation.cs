using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class LightningAnimation : Animation
{
    public LightningAnimation(Vector2 position) : base(position, "lightning", 15, 1f, 5)
    {
        
    }
}