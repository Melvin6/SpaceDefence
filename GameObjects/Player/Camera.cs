using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence;

public class Camera
{
    public Matrix Transform { get; private set; }
    public Vector2 Position { get; private set; }

    private Viewport _viewport;

    public Camera(Viewport viewport)
    {
        _viewport = viewport;
    }

    public void Update(Vector2 playerPosition)
    {
        Position = new Vector2(playerPosition.X - _viewport.Width / 2, playerPosition.Y - _viewport.Height / 2);
        Transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0);
    }
}
