using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Foster.Framework;
using FosterFlow.Graphics;
using PotatoWheel.GameObjects;

var game = new Game();
game.Run();

public class Game : App
{
   private Batcher _batcher;

   private Potato _player;

   public Game() : base(new AppConfig()
   {
      ApplicationName = "PotatoWheel",
      WindowTitle = "Potato Wheel",
      Width = 1280,
      Height = 720
   })
   {
      _batcher = new Batcher(GraphicsDevice);
   }

   protected override void Startup()
   {
      const float spriteScale = 7.0f;
      
      AtlasGenerator atlasGenerator = new("Assets", GraphicsDevice, [
         // Player animations
         Path.Combine("Potato", "PotatoIdle.ase"),
         Path.Combine("Potato", "Move", "PotatoDown.ase"),
         Path.Combine("Potato", "Move", "PotatoUp.ase"),
         Path.Combine("Potato", "Move", "PotatoMoveHorizontal.ase")
      ]);

      // Create the player sprite
      Dictionary<string, List<Subtexture>> playerAnimations = new()
      {
         {"Idle", [atlasGenerator.GetTexture("PotatoIdle0"),
                   atlasGenerator.GetTexture("PotatoIdle1")]},
         {"MoveHorizontal", [atlasGenerator.GetTexture("PotatoMoveHorizontal0"),
                             atlasGenerator.GetTexture("PotatoMoveHorizontal1"),
                             atlasGenerator.GetTexture("PotatoMoveHorizontal2")]},
         {"MoveUp", [atlasGenerator.GetTexture("PotatoUp0"),
                     atlasGenerator.GetTexture("PotatoUp1")]},
         {"MoveDown", [atlasGenerator.GetTexture("PotatoDown0"),
                       atlasGenerator.GetTexture("PotatoDown1")]}
      };
      AnimatedSprite playerSprite = new(playerAnimations, "Idle", 0.5f, spriteScale);
      _player = new Potato(playerSprite, new Vector2(Window.Width / 2.0f, (Window.Height / 2.0f) + 5));
   }

   protected override void Shutdown()
   {
      
   }

   protected override void Update()
   {
      _player.Update(Time.Delta, Input.Keyboard);
   }
   
   protected override void Render()
   {
      Window.Clear(Color.White);

      _player.Draw(_batcher);
      
      _batcher.Render(Window);
      _batcher.Clear();
   }
}