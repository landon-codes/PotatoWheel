using System.Numerics;
using Foster.Framework;
using FosterFlow;
using FosterFlow.Graphics;

var game = new Game();
game.Run();

public class Game : App
{
   private Batcher _batcher;

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
      
   }

   protected override void Shutdown()
   {
      
   }

   protected override void Update()
   {
      
   }
   
   protected override void Render()
   {
      
   }
}