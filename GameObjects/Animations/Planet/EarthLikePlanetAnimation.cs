using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class EarthLikePlanetAnimation : Animation
{
    public EarthLikePlanetAnimation(Vector2 position) : base(position, "Planet/Earth-Like planet", 15, 6f, 10, true)
    {
        
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        setFramesViaHeight();
        animation.LoadFrames(frames);
    }
}
