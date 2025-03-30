using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    internal class MultiWeapon : Weapon
    {
        public MultiWeapon(Vector2 position) : base(position, typeof(Lightning), "laser_turret")
        {
        }

        public override void Shoot(Vector2 target)
        {
            Vector2 aimDirection = LinePieceCollider.GetDirection(_position, target);
            Vector2 leftDirection = new Vector2(-aimDirection.Y, aimDirection.X);
            Vector2 rightDirection = new Vector2(aimDirection.Y, -aimDirection.X);
            Vector2 oppositeDirection = -aimDirection;
            
            ShootInDirection(aimDirection);
            ShootInDirection(leftDirection);
            ShootInDirection(rightDirection);
            ShootInDirection(oppositeDirection);
        }

        private void ShootInDirection(Vector2 direction)
        {
            GameObject newObject = (GameObject)Activator.CreateInstance(_ammo, _position, _position + direction, direction);
            GameManager.GetGameManager().AddGameObject(newObject);
        }
    }
}
