using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Foster.Framework;
using BaobabEngine.Graphics;
using PotatoWheel.GameObjects;
using PotatoWheel.UI;

var game = new Game();
game.Run();

public class Game : App
{
   private Batcher _batcher;

   // The length of the game in seconds
   private const int MatchLength = 120;
   
   private float _delayToSpawnEnemy = 2.0f;
   private float _elapsedTime;
   
   private const float SpriteScale = 7.0f;

   private Potato _player;
   private Wheel _wheel;

   private Dictionary<string, List<Subtexture>> _businessMenAnimations;
   private List<BusinessMan> _businessMen = new();

   private Text _winText;
   private Text _loseText;
   
   // The time the game has been running
   private float _matchTime = 0.0f;

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
         Path.Combine("BusyMan", "BusyManHorizontal.ase"),
         
         // Text sprites
         Path.Combine("Text", "YouWin.ase"),
         Path.Combine("Text", "GameOver.ase")
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
      
      // Create the text
      Sprite winTextSprite = new(atlasGenerator.GetTexture("YouWin"), SpriteScale);
      _winText = new Text(winTextSprite, new Vector2(Window.Width / 2.0f, Window.Height / 2.0f), false);

      Sprite loseTextSprite = new(atlasGenerator.GetTexture("GameOver"), SpriteScale);
      _loseText = new Text(loseTextSprite, new Vector2(Window.Width / 2.0f, Window.Height / 2.0f), false);
      
      // Create new enemies to start the game
      for (int i = 0; i < 5; i++)
         CreateNewEnemy();
   }

   private void CreateNewEnemy()
   {
      var sprite = new AnimatedSprite(_businessMenAnimations, "Idle", 0.3f, SpriteScale);
      
      // Generate a random position for the enemy
      var position = new Vector2(
         System.Random.Shared.Next(0, Window.Width),
         System.Random.Shared.Next(Window.Height / 2, (int)(Window.Height + sprite.Height * 2))
      );
      
      _businessMen.Add(new BusinessMan(sprite, position));
   }

   private void UpdateEnemies()
   {
      for (int i = 0; i < _businessMen.Count; i++) 
         _businessMen[i].Update(Time.Delta, _wheel.GetPosition(), _wheel, _player);
      
      // Stores the old collection of enemies to avoid changing
      // lists while iterating over them.
      var enemies = _businessMen;
      
      // Removes dead enemies
      for (int i = 0; i < _businessMen.Count; i++)
      {
         BusinessMan enemy = _businessMen[i];
         if (enemy.Dead)
            enemies.Remove(enemy);
      }

      _businessMen = enemies;
   }

   private void RenderEnemies()
   {
      for (int i = 0; i < _businessMen.Count; i++)
         _businessMen[i].Draw(_batcher);
   }

   protected override void Shutdown() { }

   protected override void Update()
   {
      // End the game if the wheel has no more health
      if (_wheel.GetHealth() <= 0)
      {
         _loseText.Show();
         return;
      }
      
      _elapsedTime += Time.Delta;
      _matchTime += Time.Delta;
      
      // Check for the win condition
      if (_matchTime >= MatchLength)
      {
         _winText.Show();
         return;
      }
      
      // Check if a new enemy should be spawned
      if (_elapsedTime >= _delayToSpawnEnemy)
      {
         _elapsedTime -= _delayToSpawnEnemy;
         
         // Slowly makes enemies spawn faster
          _delayToSpawnEnemy -= (_delayToSpawnEnemy >= 0.9f) ? 0.03f : 0.0f;
         
         CreateNewEnemy();
      }
      
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
      
      _loseText.Draw(_batcher);
      _winText.Draw(_batcher);
      
      _batcher.Render(Window);
      _batcher.Clear();
   }
}