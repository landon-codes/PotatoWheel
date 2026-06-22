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
   
   private const float SpriteScale = 7.0f;

   private Potato _player;
   private Wheel _wheel;

   private Dictionary<string, List<Subtexture>> _businessMenAnimations;
   private List<BusinessMan> _businessMen = new();

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
      
      
      AtlasGenerator atlasGenerator = new("Assets", GraphicsDevice, [
         // Player animations
         Path.Combine("Potato", "PotatoIdle.ase"),
         Path.Combine("Potato", "Move", "PotatoDown.ase"),
         Path.Combine("Potato", "Move", "PotatoUp.ase"),
         Path.Combine("Potato", "Move", "PotatoMoveHorizontal.ase"),
         Path.Combine("Potato", "PotatoSpin.ase"),
         
         // Wheel animations
         Path.Combine("Wheel", "WheelIdle.ase"),
         Path.Combine("Wheel", "WheelSpin.ase"),
         
         // Businessmen animations
         Path.Combine("BusyMan", "BusyManIdle.ase"),
         Path.Combine("BusyMan", "BusyManHorizontal.ase")
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
      AnimatedSprite playerSprite = new(playerAnimations, "Idle", 0.5f, SpriteScale);
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
      AnimatedSprite wheelSprite = new(wheelAnimations, "Idle", 0.1f, SpriteScale);
      _wheel = new Wheel(wheelSprite, new Vector2(Window.Width / 2.0f, Window.Height * (1.0f / 4.0f)) );
      
      // Define the animations for businessmen
      _businessMenAnimations = new Dictionary<string, List<Subtexture>>()
      {
         {"Idle", [atlasGenerator.GetTexture("BusyManIdle")]},
         {"Move", [atlasGenerator.GetTexture("BusyManHorizontal0"),
                   atlasGenerator.GetTexture("BusyManHorizontal1")]}
      };
      
      CreateNewEnemy();
   }

   private void CreateNewEnemy()
   {
      // TODO: Create logic for randomizing the position of enemies.
      var position = new Vector2(Window.Width / 2.0f, Window.Height / 2.0f);
      var sprite = new AnimatedSprite(_businessMenAnimations, "Idle", 0.3f, SpriteScale);
      
      _businessMen.Add(new BusinessMan(sprite, position));
   }

   private void UpdateEnemies()
   {
      for (int i = 0; i < _businessMen.Count; i++) 
         _businessMen[i].Update(Time.Delta, _wheel.GetPosition(), _wheel);
   }

   private void RenderEnemies()
   {
      for (int i = 0; i < _businessMen.Count; i++)
         _businessMen[i].Draw(_batcher);
   }

   protected override void Shutdown() { }

   protected override void Update()
   {
      _player.Update(Time.Delta, Input.Keyboard);
      UpdateEnemies();
      _wheel.Update(Time.Delta);
   }
   
   protected override void Render()
   {
      Window.Clear(Color.White);
      
      _wheel.Draw(_batcher);
      RenderEnemies();
      _player.Draw(_batcher);
      
      _batcher.Render(Window);
      _batcher.Clear();
   }
}