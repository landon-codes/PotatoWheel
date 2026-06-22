using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class BusinessMan(AnimatedSprite sprite, Vector2 startingPosition)
{
    private int _health = 100;
    
    // Amount of time in seconds that needs to pass before damage can be taken again
    private const float DamageDelay = 0.25f;
    private float _elapsedTime;
    
    private Vector2 _position = startingPosition;

    public CircleBound Bounds => new(_position, sprite);

    private void TakeDamage(int damage)
    {
        if (!(_elapsedTime > DamageDelay)) 
            return;

        _health -= damage;
        _elapsedTime = 0;
    }

    public bool Dead => _health <= 0;

    public void Update(float deltaTime, Vector2 wheelPosition, in Wheel wheel, in Potato player)
    {
        const int movementSpeed = 30;

        _elapsedTime += deltaTime;
        
        // Go towards the wheel
        var deltaDistance = wheelPosition - _position;
        var direction = Vector2.Zero;
        
        if (deltaDistance.LengthSquared() > 0f)
            direction = Vector2.Normalize(deltaDistance);

        _position += (direction * movementSpeed) * deltaTime;
        
        // Update sprite
        sprite.PlayAnimation(direction == Vector2.Zero ? "Idle" : "Move", false);
        sprite.Update(deltaTime);
        
        // Check for player attacks
        if (player.Bounds.Intersects(Bounds) && player.IsSpinning())
            TakeDamage(50);
            
        // Check for collisions
        if (wheel.Bounds.Intersects(Bounds))
            wheel.TakeDamage(5);
    }

    public void Draw(Batcher batcher)
    {
        sprite.Draw(batcher, _position);
    }
}