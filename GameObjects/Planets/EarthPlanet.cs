using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class EarthPlanet : Planet
{
    public EarthPlanet(Vector2 position) : base(new EarthLikePlanetAnimation(position), position, "Earth like planet")
    {
    }
}