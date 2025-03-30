using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class AlienPlanet : Planet
{
    public AlienPlanet(Vector2 position) : base(new AlienPlanetAnimation(position), position , "Alien planet")
    {
    }
}