using BaobabEngine.Graphics;
using System.Numerics;
using Foster.Framework;

namespace PotatoWheel.UI;

public class Icon(Sprite sprite, Vector2 position)
{
    public bool IsVisible = false;
    private float _timeVisible;

    public void Update(float deltaTime)
    {
        _timeVisible += deltaTime;
        
        const float visibleTimer = 3.0f;
        if (_timeVisible >= visibleTimer)
        {
            _timeVisible = 0.0f;
            IsVisible = false;
        }
    }
    
    public void Draw(Batcher batcher)
    {
        if (!IsVisible) return;
        
        sprite.Draw(batcher, position);
    }
}