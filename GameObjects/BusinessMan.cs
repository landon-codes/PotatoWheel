using Foster.Framework;
using FosterFlow.Graphics;
using FosterFlow.Collisions;
using System.Numerics;

namespace PotatoWheel.GameObjects;

public class BusinessMan(AnimatedSprite sprite, Vector2 startingPosition)
{
    private Vector2 _position = startingPosition;

    public CircleBound Bounds => new(_position, sprite);

    public void Update(float deltaTime)
    {
        sprite.Update(deltaTime);
    }

    public void Draw(Batcher batcher)
    {
        sprite.Draw(batcher, _position);
    }
}