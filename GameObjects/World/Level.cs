using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceDefence;
public abstract class Level : GameObject
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Rectangle Bounds { get; protected set; }
   
    protected List<GameObject> _gameObjects;
    protected Texture2D _backgroundTexture;

    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        Bounds = new Rectangle(-Width / 2, -Height / 2, Width, Height);
        _gameObjects = new List<GameObject>();
    }

    public abstract void Setup();

    public void Start()
    {
        Setup();
        foreach (GameObject gameObject in _gameObjects) {
            GameManager.GetGameManager().AddGameObject(gameObject);
        }
    }

    public void LoadContent(ContentManager content, string backgroundTexturePath)
    {
        _backgroundTexture = content.Load<Texture2D>(backgroundTexturePath);
    }

    public List<GameObject> getGameObjects()
    {
        return _gameObjects;
    }

    public List<GameObject> End()
    {
        List<GameObject> gameObjects = getGameObjects();
        _gameObjects = new List<GameObject>();
        return gameObjects;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (_backgroundTexture != null)
        {
            spriteBatch.Draw(_backgroundTexture, Bounds, Color.White);
        }
    }
}
