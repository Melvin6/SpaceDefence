using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class BulletWeapon : Weapon
    {
        public BulletWeapon(Vector2 position) : base(position, typeof(Bullet), "base_turret")
        {
        }
    }
}
