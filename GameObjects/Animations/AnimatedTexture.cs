//Source: https://docs.monogame.net/articles/getting_to_know/howto/graphics/HowTo_Animate_Sprite.html

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence;

/// <summary>
/// A helper class for handling animated textures.
/// </summary>
public class AnimatedTexture
{
    private int frameCount;
    public Texture2D myTexture;
    private float timePerFrame;
    private int frame;
    private float totalElapsed;
    private bool isPaused;
    public float Rotation, Scale, Depth;
    public Vector2 Origin;
    public int FrameWidth {get; private set;}

    public AnimatedTexture(Vector2 origin, float rotation, float scale, float depth)
    {
        this.Origin = origin;
        this.Rotation = rotation;
        this.Scale = scale;
        this.Depth = depth;
    }

    public void Load(ContentManager content, string asset, int framesPerSec, int frames)
    {
        frameCount = frames;
        myTexture = content.Load<Texture2D>(asset);
        timePerFrame = (float)1 / framesPerSec;
        frame = 0;
        totalElapsed = 0;
        isPaused = false;

        this.FrameWidth = myTexture.Width / frameCount;
    }

    public void UpdateFrame(float elapsed)
    {
        if (isPaused)
            return;
        totalElapsed += elapsed;
        if (totalElapsed > timePerFrame)
        {
            frame++;
            frame %= this.frameCount;
            totalElapsed -= timePerFrame;
        }
    }

    public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
    {
        DrawFrame(batch, frame, screenPos);
    }

    public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
    {
        int FrameWidth = myTexture.Width / frameCount;
        Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
            FrameWidth, myTexture.Height);
        batch.Draw(myTexture, screenPos, sourcerect, Color.White,
            Rotation, Origin, Scale, SpriteEffects.None, Depth);
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public void Reset()
    {
        frame = 0;
        totalElapsed = 0f;
    }

    public void Stop()
    {
        Pause();
        Reset();
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public int CurrentFrame() {return frame;}

    public void LoadFrames(int frames) 
    {
        this.frameCount = frames;
        this.FrameWidth = myTexture.Width / frameCount;
    }
}
