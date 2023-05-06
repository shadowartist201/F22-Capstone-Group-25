using System;
namespace Game_Demo
{
	public struct Item
	{
		public string name;
		public string about;
		public Item()
		{
			name = "Name";
			about = "About";
		}
		public Item(string n, string a)
		{
			name = n;
			about = a;
		}

	}
}

