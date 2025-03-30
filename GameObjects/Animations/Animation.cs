//Source: https://docs.monogame.net/articles/getting_to_know/howto/graphics/HowTo_Animate_Sprite.html
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;
public class Animation : GameObject
{
    // The reference to the AnimatedTexture for the character
    public AnimatedTexture animation;
    // The rotation of the character on screen
    private const float rotation = 0;
    // The scale of the character, how big it is drawn
    private float scale;
    // The draw order of the sprite
    private const float depth = 0.5f;

    private Vector2 _position;
    private bool isActive;
    private bool loop;


    private string spriteSheet;
    public int frames {get; private set;}
    private int framesPerSec;


    public Animation(Vector2 position, string spriteSheet, int fps, float scale, int frames = 0, bool loop = false)
    {
        this.scale = scale;
        _position = position;
        this.spriteSheet = spriteSheet;
        framesPerSec = fps;
        this.frames = frames;
        this.loop = loop;

        animation = new AnimatedTexture(Vector2.Zero, rotation, scale, depth);
        this.isActive = false;
    }

    public override void Load(ContentManager content)
    {
        animation.Load(content, spriteSheet, framesPerSec, frames);
    }

    public override void Update(GameTime gameTime)
    {
        if (isActive)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animation.UpdateFrame(elapsed);
            if (animation.CurrentFrame() == frames - 1) 
            {
                if (loop) Play();
                else Stop();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (isActive)
        {
            animation.DrawFrame(spriteBatch, _position);
        }
    }

    public void Play()
    {
        isActive = true;
        animation.Play();
    }

    public void Stop()
    {
        isActive = false;
        animation.Stop();
    }

    public void updatePosition(Vector2 position)
    {
        _position = position;
    }

    public void setFramesViaHeight()
    {
        if (animation.myTexture != null)
        {
            this.frames = animation.myTexture.Width / animation.myTexture.Height;
        }
    }
}
