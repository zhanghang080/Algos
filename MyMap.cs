using System;
using System.Collections.Generic;
using System.Text;

namespace algo
{
	public class MyMap
	{
		
		public void run()
		{
			
			this.open_set.Add(new TupleExtend(this.start.Item1, this.start.Item2));
			TupleExtend startpoint = new TupleExtend(this.start.Item1, this.start.Item2);
			startpoint.cost = 0;
			
			while (true)
			{
				int index = this.SelectPointInOpenList();
				if (index < 0)
				{
					Console.WriteLine("找不到路径，失败");
					return;
				}
				TupleExtend p = this.open_set[index];
				string str = string.Format("选中点:({0},{1})", p.Item1, p.Item2);
				Console.WriteLine(str);
				if (this.isEndPoint(p))
				{
					Console.WriteLine("成功");
					this.BuildPath(p);
					return;
				}
				this.open_set.RemoveAt(index);
				this.close_set.Add(p);

				this.ProcessPoint(new TupleExtend(p.Item1 - 1, p.Item2 + 1), p);
				this.ProcessPoint(new TupleExtend(p.Item1 - 1, p.Item2), p);
				this.ProcessPoint(new TupleExtend(p.Item1 - 1, p.Item2 - 1), p);
				this.ProcessPoint(new TupleExtend(p.Item1, p.Item2 - 1), p);
				this.ProcessPoint(new TupleExtend(p.Item1 + 1, p.Item2 - 1), p);
				this.ProcessPoint(new TupleExtend(p.Item1 + 1, p.Item2), p);
				this.ProcessPoint(new TupleExtend(p.Item1 + 1, p.Item2 + 1), p);
				this.ProcessPoint(new TupleExtend(p.Item1, p.Item2 + 1), p);
				Console.WriteLine("=================");
			}
		}

		/// <summary>
		/// 从open_set中找到优先级最高(TotalCost最小)的节点，返回其索引。
		/// </summary>
		/// <returns></returns>
		private int SelectPointInOpenList()
		{
			int index = 0;
			int selected_index = -1;
			double mincost = 999999;
			foreach (var p in this.open_set)
			{
				double cost = this.TotalCost((p.Item1, p.Item2));   //f(n)
				if (cost < mincost)
				{
					mincost = cost;
					selected_index = index;
				}
				index += 1;
			}
			return selected_index;
		}


		/// <summary>
		/// 针对每一个节点进行处理：如果是没有处理过的节点，则计算优先级设置父节点，并且添加到open_set中。
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="parent"></param>
		private void ProcessPoint(TupleExtend p1, TupleExtend parent)
		{
			if (this.isValidPoint((p1.Item1, p1.Item2)) == false)
			{
				return;
			}
			if (this.isInCloseList(new TupleExtend(p1.Item1, p1.Item2)))
			{
				return;
			}
			p1.cost = this.TotalCost((p1.Item1, p1.Item2));
			string str = string.Format("处理点({0},{1}),优先级:{2}", p1.Item1, p1.Item2, p1.cost);
			Console.WriteLine(str);
			if (this.isInOpenList(new TupleExtend(p1.Item1, p1.Item2)) == false)
			{
				p1.parent = parent;
				p1.cost = this.TotalCost((p1.Item1, p1.Item2));
				this.open_set.Add(p1);
			}
		}

		/// <summary>
		/// 从终点往回沿着parent构造结果路径
		/// </summary>
		/// <param name="p"></param>
		private void BuildPath(TupleExtend p)
		{
			List<TupleExtend> path = new List<TupleExtend>();
			while (true)
			{
				path.Insert(0, p);
				if (this.isStartPoint(p))
				{
					break;
				}
				else
				{
					p = p.parent;
				}
			}
			foreach (var point in path)
			{
				this.mp[point.Item2][point.Item1] = 6;
			}
		}








		//(列，行)
		public ValueTuple<int, int> start;
		public ValueTuple<int, int> end;
		public int[][] mp;
		public int row;
		public int column;
		public List<TupleExtend> open_set = new List<TupleExtend>();// 待遍历的节点
		public List<TupleExtend> close_set = new List<TupleExtend>();//已经遍历过的节点
		/// <summary>
		/// 默认起点在左下角，终点在右上角
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		public MyMap(int row,int column)
		{
			this.mp = new int[row][];
			for(int i =0;i< row;i++)
			{
				this.mp[i] = new int[column];
			}
			this.row = row;
			this.column = column;
			this.start = (0, row - 1);
			this.end = (column - 1,0);
			for(int i =0;i<row;i++)
			{
				for(int j =0;j<column;j++)
				{
					this.mp[i][j] = 1;
				}
			}
		}
		public MyMap(int row, int column,(int,int) start,(int,int) end)
		{
			this.mp = new int[row][];
			for (int i = 0; i < row; i++)
			{
				this.mp[i] = new int[column];
			}
			this.row = row;
			this.column = column;
			this.start = start;
			this.end = end;
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < column; j++)
				{
					this.mp[i][j] = 1;
				}
			}
		}
		// f(n) = g(n) + h(n)

		/// <summary>
		/// g(n) 节点到起点的移动代价  ,,节点n距离起点的代价
		/// </summary>
		/// <param name="p2"></param>
		/// <returns></returns>
		private double baseCost( ValueTuple<int, int> p2)
		{   
			int x_dis = Math.Abs(p2.Item1-this.start.Item1);
			int y_dis = Math.Abs(p2.Item2-this.start.Item2);
			int smaller= Math.Min(x_dis, y_dis);
			double ret = x_dis + y_dis + (Math.Sqrt(2) - 2) * smaller;
			return ret;
			
		}

		/// <summary>
		/// h(n)  节点到终点的启发函数 ,,节点n距离终点的预计代价
		/// </summary>
		/// <param name="p2"></param>
		/// <returns></returns>
		private double HeuristicCost(ValueTuple<int, int> p2)
		{ 
			int x_dis = Math.Abs(p2.Item1- this.end.Item1 );
			int y_dis = Math.Abs(p2.Item2- this.end.Item2 );
			int smaller = Math.Min(x_dis, y_dis);
			double ret = x_dis + y_dis + (Math.Sqrt(2) - 2) * smaller;
			return ret;
		}

		/// <summary>
		/// f(n) 代价总和  节点n的综合优先级。当我们选择下一个要遍历的节点时，我们总会选取综
		/// 合优先级最高（值最小）的节点。
		/// </summary>
		/// <param name="p2"></param>
		/// <returns></returns>
		public double TotalCost(ValueTuple<int, int> p2)
		{
			
			return this.baseCost(p2) + this.HeuristicCost(p2);
		}

		/// <summary>
		/// 判断点是否有效，不在地图内部或者障碍物所在点都是无效的。
		/// 障碍物即二维数组中某一个成员的值为0
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool isValidPoint(ValueTuple<int,int> p)
		{
			
			if(p.Item1 < 0 || p.Item2 < 0)
			{
				return false;
			}
			if(p.Item1 >=this.column || p.Item2 >=this.row)
			{
				return false;
			}
			if(this.mp[p.Item2][p.Item1] ==0 )
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 判断点是否在某个集合中
		/// </summary>
		/// <param name="p"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		private bool isInPointList(TupleExtend p, List<TupleExtend> list)
		{
			foreach( var point in list  )
			{
				if(p.Item1 == point.Item1 && p.Item2 == point.Item2)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 判断点是否在open_set中。
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool isInOpenList(TupleExtend p)
		{
			return this.isInPointList(p, this.open_set);
		}
		/// <summary>
		/// 判断点是否在close_set中。
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool isInCloseList(TupleExtend p)
		{
			return this.isInPointList(p, this.close_set);
		}
		/// <summary>
		/// 判断点是否是起点
		/// </summary>
		private bool isStartPoint(TupleExtend p)
		{
			if(p.Item1 == this.start.Item1 && p.Item2 == this.start.Item2)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 判断点是否是终点
		/// </summary>
		private bool isEndPoint(TupleExtend p)
		{
			if (p.Item1 == this.end.Item1 && p.Item2 == this.end.Item2)
			{
				return true;
			}
			return false;
		}


		

		

		
	}
}
