using System;

namespace algo
{
	class Program
	{
		static void Main(string[] args)
		{

			MyMap mp = new MyMap(8, 8);
			for(int i = 2;i <=5;i++)
			{
				for(int j =1;j<=5;j++)
				{
					mp.mp[i][j] = 0;
				}
			}
			mp.mp[6][6] = 0;
			mp.mp[5][6] = 0;
			mp.run();
			printMap(mp);
		}
		static void printMap(MyMap mp)
		{
			for (int i = 0; i < mp.mp.Length; i++)
			{
				for (int j = 0; j < mp.mp[i].Length; j++)
				{
					Console.Write(mp.mp[i][j] + " ");
				}
				Console.WriteLine();
			}
		}
	}
}
