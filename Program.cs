using Game_Demo;

Game1 game1 = new Game1();

game1.Run();

/*
Battle notes
-----------------------------------

total enemy hp = 0, battle win
total party hp = 0, battle lose
-------------------
attack message

us: set chara to us and enemy to them
them: set chara to them and enemy to cat
	when cat dead, set enemy to nobody
attack
	attack either them or us, then end turn
----------------------
magic message

us: set chara to us and enemy to them
them: set chara to them and enemy to us
magic
	magic either them or us, then end turn
	if no magic, continue turn
cat magic
	heal nobody, then end turn
-------------------------
item message
	activate effect based on item, then end turn

*/