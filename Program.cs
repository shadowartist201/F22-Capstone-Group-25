using System;

/*Console.WriteLine("------Nobody Demos------");
Console.WriteLine("\n");
Console.WriteLine("1) World Demo - Experience movement and 3D audio");
Console.WriteLine("2) Battle Demo - Fight against a dragon in turn-based combat");
Console.WriteLine("\n");
Console.WriteLine("Your choice: ");

string option = Console.ReadLine();*/
namespace Game_Demo
{
    public static class Program
    {
        public static Game1 Game;
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
            {
                Game = game;
                Game.Run();
            }
        }
    }
}