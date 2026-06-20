using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class Potato(AnimatedSprite sprite, Vector2 startingPosition)
{
    private Vector2 _position = startingPosition;

    // I'm too lazy to implement an enum.
    private string _direction = "right";

    public CircleBound Bounds => new CircleBound(_position, sprite);
    
    public void Update(float deltaTime, KeyboardState keyboard)
    {
        sprite.Update(deltaTime);
        
        // Implement movement
        const int movementSpeed = 10;

        bool moving = false;
        
        if (keyboard.Down(Keys.Up) || keyboard.Down(Keys.W))
        {
            moving = true;
            
            _position.Y -= movementSpeed;
            sprite.PlayAnimation("MoveUp", false);
        }
        if (keyboard.Down(Keys.Down) || keyboard.Down(Keys.S))
        {
            moving = true;
            
            _position.Y += movementSpeed;
            sprite.PlayAnimation("MoveDown", false);
        }

        if (keyboard.Down(Keys.Right) || keyboard.Down(Keys.D))
        {
            moving = true;
            
            _position.X += movementSpeed;
            _direction = "right";
            sprite.PlayAnimation("MoveHorizontal", false);
        }
        if (keyboard.Down(Keys.Left) || keyboard.Down(Keys.A))
        {
            moving = true;
            
            _position.X -= movementSpeed;
            _direction = "left";
            sprite.PlayAnimation("MoveHorizontal", false);
        }

        if (!moving)
        {
            _direction = "right";
            sprite.PlayAnimation("Idle", false);
        }
    }

    public void Draw(Batcher batcher)
    {
        if (_direction == "right")
            sprite.Draw(batcher, _position);
        else 
            sprite.Draw(batcher, _position, true, false);
    }
}