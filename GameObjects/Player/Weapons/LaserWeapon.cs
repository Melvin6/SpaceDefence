using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class LaserWeapon : Weapon
    {
        public LaserWeapon(Vector2 position) : base(position, typeof(Laser), "laser_turret")
        {
        }
    }
}
