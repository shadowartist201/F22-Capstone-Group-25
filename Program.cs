
using Game_Demo;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

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
    //Battle _battle = new Battle();

    //new Entity(name, HP, max HP, MP, max MP, attack, magic attack, defense, magic defense, speed)

    List<Entity> enemies = new List<Entity>();
    enemies.Add(new Entity("Dragon", 200, 200, 0, 0, 10, 15, 0, 0, 5));

    List<Entity> squad = new List<Entity>();
    squad.Add(new Entity("Nobody", 100, 100, 10, 10, 5, 10, 10, 12, 10));
    squad.Add(new Entity("Cat", 50, 50, 0, 0, 5, 10, 5, 7, 15));

    Battle _battle = new Battle(enemies, squad);
    _battle.Run();
}