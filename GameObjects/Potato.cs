using Foster.Framework;
using BaobabEngine.Graphics;
using BaobabEngine.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class Potato(AnimatedSprite sprite, Vector2 startingPosition)
{
    private Vector2 _position = startingPosition;

    // Accessed externally for attacking
    private bool _spinning = false;

    // I'm too lazy to implement an enum.
    private string _direction = "right";

    public CircleBound Bounds => new CircleBound(_position, sprite);

    public Vector2 GetPosition()
    {
        return _position;
    }

    public bool IsSpinning()
    {
        return _spinning;
    }
    
    public void Update(float deltaTime, KeyboardState keyboard)
    {
        sprite.Update(deltaTime);

        const int movementSpeed = 400;
        bool moving = false;
        string animationToPlay = "none";

        // Vertical
        if (keyboard.Down(Keys.Up) || keyboard.Down(Keys.W))
        {
            moving = true;
            _position.Y -= movementSpeed * deltaTime;
            animationToPlay = "MoveUp";
        }
        else if (keyboard.Down(Keys.Down) || keyboard.Down(Keys.S))
        {
            moving = true;
            _position.Y += movementSpeed * deltaTime;
            animationToPlay = "MoveDown";
        }

        // Horizontal
        if (keyboard.Down(Keys.Right) || keyboard.Down(Keys.D))
        {
            moving = true;
            _position.X += movementSpeed * deltaTime;
            _direction = "right";

           // Don't override vertical animations
            if (animationToPlay == "none")
                animationToPlay = "MoveHorizontal";
        }
        else if (keyboard.Down(Keys.Left) || keyboard.Down(Keys.A))
        {
            moving = true;
            _position.X -= movementSpeed * deltaTime;
            _direction = "left";

            if (animationToPlay == "none")
                animationToPlay = "MoveHorizontal";
        }

        if (keyboard.Down(Keys.Space))
        {
            moving = true;
            animationToPlay = "Spin";

            _spinning = true;
        }
        else _spinning = false;

        if (!moving)
        {
            _direction = "right";
            animationToPlay = "Idle";
        }
        
        if (animationToPlay != "none")
            sprite.PlayAnimation(animationToPlay, false);
    }
    
    public void Draw(Batcher batcher)
    {
        if (_direction == "right")
            sprite.Draw(batcher, _position);
        else 
            sprite.Draw(batcher, _position, true, false);
    }
}