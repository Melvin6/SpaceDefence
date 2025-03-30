using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class AlienPlanetAnimation : Animation
{
    public AlienPlanetAnimation(Vector2 position) : base(position, "Planet/Alien planet", 10, 4f, 10, true)
    {
        
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        setFramesViaHeight();
        animation.LoadFrames(frames);
    }
}
