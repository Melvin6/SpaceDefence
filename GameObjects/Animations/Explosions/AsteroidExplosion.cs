//Source: https://docs.monogame.net/articles/getting_to_know/howto/graphics/HowTo_Animate_Sprite.html
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class AsteroidExplosion : Animation
{
    public AsteroidExplosion(Vector2 position) : base(position,"Asteroid 01 - Explode", 8, 1f, 8)
    {
    }
}
