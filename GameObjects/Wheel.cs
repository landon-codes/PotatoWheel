using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class Wheel(AnimatedSprite sprite, Vector2 position)
{
    private int _health = 100;

    public CircleBound Bounds => new CircleBound(position, sprite.Width - 2);
    
    // Amount of time in seconds that needs to pass before damage can be taken again
    private const float DamageDelay = 1;
    private float _elapsedTime = 0;

    public void TakeDamage(int damage)
    {
        if (!(_elapsedTime > DamageDelay))
            return;
        
        _health -= damage;
        _elapsedTime = 0;
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    // Returns the health for use in losing checks
    public int GetHealth()
    {
        return _health;
    }

    public void Update(float deltaTime)
    {
        sprite.Update(deltaTime);
        _elapsedTime += deltaTime;
        System.Console.WriteLine(_health);
    }

    public void Draw(Batcher batcher)
    {
        sprite.Draw(batcher, position);
    }
}