using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceDefence.Collision;

namespace SpaceDefence;
internal abstract class Ammo : GameObject
{
    protected Texture2D _texture;
    protected Collider _collider;
    protected Vector2 _velocity;
    protected double _lifespan;

    public Ammo(Collider collider, Vector2 velocity, double lifespan)
    {
        _collider = collider;
        _velocity = velocity;
        _lifespan = lifespan;
        SetCollider(_collider);
    }
}
