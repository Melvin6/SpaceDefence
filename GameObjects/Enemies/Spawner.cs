using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;

namespace SpaceDefence;
public class Spawner : GameObject {
    public Type _type {get; private set;}
    private float _timer;
    private float _intervals;

    public Spawner( Type type, float interval){
        _type = type;
        _timer = 0;
        _intervals = interval;
    }

    public override void Update(GameTime gameTime)
    {
        _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer <= 0) 
        {
            Spawn();
            _timer = _intervals;
        }

        base.Update(gameTime);
    }

    public void Spawn()
    {
        GameObject newObject = (GameObject)Activator.CreateInstance(_type);
        GameManager.GetGameManager().AddGameObject(newObject);
    }
}
   