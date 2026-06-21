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
   private Wheel _wheel;

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
         Path.Combine("Potato", "Move", "PotatoMoveHorizontal.ase"),
         Path.Combine("Potato", "PotatoSpin.ase"),
         
         // Wheel animations
         Path.Combine("Wheel", "WheelIdle.ase"),
         Path.Combine("Wheel", "WheelSpin.ase")
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
                       atlasGenerator.GetTexture("PotatoDown1")]},
         {"Spin", [atlasGenerator.GetTexture("PotatoSpin0"),
                   atlasGenerator.GetTexture("PotatoSpin1"),
                   atlasGenerator.GetTexture("PotatoSpin2"),
                   atlasGenerator.GetTexture("PotatoSpin3")]}
      };
      AnimatedSprite playerSprite = new(playerAnimations, "Idle", 0.5f, spriteScale);
      _player = new Potato(playerSprite, new Vector2(Window.Width / 2.0f, (Window.Height / 2.0f) + 5));
      
      // Create the wheel 
      var wheelAnimations = new Dictionary<string, List<Subtexture>>()
      {
         {"Idle", [atlasGenerator.GetTexture("WheelIdle")]},
         {"Spin", [atlasGenerator.GetTexture("WheelSpin0"),
                     atlasGenerator.GetTexture("WheelSpin1"),
                     atlasGenerator.GetTexture("WheelSpin2"),
                     atlasGenerator.GetTexture("WheelSpin3")]}
      };
      AnimatedSprite wheelSprite = new(wheelAnimations, "Idle", 0.1f, spriteScale);
      _wheel = new Wheel(wheelSprite, new Vector2(Window.Width / 2.0f, Window.Height * (1.0f / 4.0f)) );
   }

   protected override void Shutdown()
   {
      
   }

   protected override void Update()
   {
      _player.Update(Time.Delta, Input.Keyboard);
      _wheel.Update(Time.Delta);
   }
   
   protected override void Render()
   {
      Window.Clear(Color.White);
      
      _wheel.Draw(_batcher);
      _player.Draw(_batcher);
      
      _batcher.Render(Window);
      _batcher.Clear();
   }
}