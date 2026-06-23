using BaobabEngine.Graphics;
using System.Numerics;
using Foster.Framework;

namespace PotatoWheel.UI;

public class Text(Sprite sprite, Vector2 position, bool isVisible)
{
    private bool _isVisible = isVisible;

    public void Show()
    {
        _isVisible = true; 
    }

    public void Hide()
    {
        _isVisible = false;
    }
    
    public void Draw(Batcher batcher)
    {
        if (!_isVisible) return;
        
        sprite.Draw(batcher, position);
    }
}