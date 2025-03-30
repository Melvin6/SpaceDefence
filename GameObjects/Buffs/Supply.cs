using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class Supply : Buff
    {
        public Supply() : base (new LaserWeapon(Vector2.Zero), "Crate")
        {
            
        }
    }
}
