using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class BusinessMan(AnimatedSprite sprite, Vector2 startingPosition)
{
    private Vector2 _position = startingPosition;

    public CircleBound Bounds => new(_position, sprite);

    public void Update(float deltaTime, Vector2 wheelPosition)
    {
        const int movementSpeed = 30;
        
        sprite.Update(deltaTime);
        
        // Go towards the wheel
        var deltaDistance = wheelPosition - _position;
        var direction = Vector2.Zero;
        
        if (deltaDistance.LengthSquared() > 0f)
            direction = Vector2.Normalize(deltaDistance);

        _position += (direction * movementSpeed) * deltaTime;
    }

    public void Draw(Batcher batcher)
    {
        sprite.Draw(batcher, _position);
    }
}