using Foster.Framework;
using BaobabEngine.Graphics;
using BaobabEngine.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class Potato(AnimatedSprite sprite, Vector2 startingPosition)
{
    private Vector2 _position = startingPosition;

    // Accessed externally for attacking
    private bool _spinning;

    // I'm too lazy to implement an enum.
    private string _direction = "right";

    public int Damage { get; private set; } = 25;

    private int _movementSpeed = 400;

    public CircleBound Bounds => CalculateBounds();

    private CircleBound CalculateBounds()
    {
        var bound = new CircleBound(_position, sprite);
        bound.ScaleBounds(1.25f);
        return bound;
    }

    public Vector2 GetPosition()
    {
        return _position;
    }

    public void IncreaseDamage(int increase)
    {
        Damage += increase;
    }

    public void IncreaseSpeed(int increase)
    {
        _movementSpeed += increase;
    }

    public bool IsSpinning()
    {
        return _spinning;
    }
    
    public void Update(float deltaTime, KeyboardState keyboard)
    {
        sprite.Update(deltaTime);

        // Stores the change in distance
        Vector2 motion = new();
        
        bool moving = false;
        string animationToPlay = "none";

        // Vertical
        if (keyboard.Down(Keys.Up) || keyboard.Down(Keys.W))
        {
            moving = true;
            motion.Y -= _movementSpeed * deltaTime;
            animationToPlay = "MoveUp";
        }
        else if (keyboard.Down(Keys.Down) || keyboard.Down(Keys.S))
        {
            moving = true;
            motion.Y += _movementSpeed * deltaTime;
            animationToPlay = "MoveDown";
        }

        // Horizontal
        if (keyboard.Down(Keys.Right) || keyboard.Down(Keys.D))
        {
            moving = true;
            motion.X += _movementSpeed * deltaTime;
            _direction = "right";

           // Don't override vertical animations
            if (animationToPlay == "none")
                animationToPlay = "MoveHorizontal";
        }
        else if (keyboard.Down(Keys.Left) || keyboard.Down(Keys.A))
        {
            moving = true;
            motion.X -= _movementSpeed * deltaTime;
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

        // Apply player speed
        // Reduces speed when spinning
        _position += (_spinning) ? motion * 0.25f : motion;
    }
    
    public void Draw(Batcher batcher)
    {
        if (_direction == "right")
            sprite.Draw(batcher, _position);
        else 
            sprite.Draw(batcher, _position, true, false);
    }
}