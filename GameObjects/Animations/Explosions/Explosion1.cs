using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class Explosion1 : Animation
{
    public Explosion1(Vector2 position) : base(position, "Explosion", 15, 1f, 40)
    {
        
    }
}
