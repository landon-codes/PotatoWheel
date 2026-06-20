using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class Wheel(AnimatedSprite sprite, Vector2 position)
{
    private int _health = 100;

    public CircleBound Bounds => new CircleBound(position, sprite.Width - 2);

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }


    // Returns the health for use in losing checks
    public int GetHealth()
    {
        return _health;
    }

    public void Draw(Batcher batcher)
    {
        sprite.Draw(batcher, position);
    }
}