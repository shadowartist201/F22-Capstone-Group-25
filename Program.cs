
using Game_Demo;
using System.Collections.Specialized;
using System;

//I'm not sure why, but you can't have both _world and _battle defined at the same time

Console.WriteLine("------Nobody Demos------");
Console.WriteLine("\n");
Console.WriteLine("1) World Demo - Experience movement and 3D audio");
Console.WriteLine("2) Battle Demo - Fight against a dragon in turn-based combat");
Console.WriteLine("\n");
Console.WriteLine("Your choice: ");

string option = Console.ReadLine();

if (option == "1")
{
    World _world = new World();
    _world.Run();
}
else if (option == "2")
{
    Battle _battle = new Battle();
    _battle.Run();
}