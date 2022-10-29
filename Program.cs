
using Game_Demo;
using System.Collections.Specialized;
using System;

World _world = new World();
Battle _battle = new Battle();

Console.WriteLine("------Nobody Demos------");
Console.WriteLine("\n");
Console.WriteLine("1) World Demo - Experience movement and 3D audio");
Console.WriteLine("2) Battle Demo - Fight against a dragon in turn-based combat");
Console.WriteLine("\n");
Console.WriteLine("Your choice: ");

string option = Console.ReadLine();

if (option == "1")
{
    _world.Run();
}
else if (option == "2")
{
    _battle.Run();
}