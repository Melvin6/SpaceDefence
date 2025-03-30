using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceDefence;
public class Level1 : Level
{
    public Level1() : base(6000, 3000) { }

    public override void Setup()
    {
        _gameObjects.Add(new EarthPlanet(new Vector2(1800, -900)));
        _gameObjects.Add(new AlienPlanet( new Vector2(-2000, 800)));
        _gameObjects.Add(new Supply());
        _gameObjects.Add(new MultiSupply());
        _gameObjects.Add(new Spawner(typeof(Alien), 5));
        _gameObjects.Add(new Spawner(typeof(Asteroid), 5));  
        LoadContent(GameManager.GetGameManager()._content, "stars_texture");
    }
}
