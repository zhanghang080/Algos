using System;
using System.Collections.Generic;
using System.Text;

namespace algo
{
	public class TupleExtend : Tuple<int, int>
	{
		public TupleExtend parent;
		public double cost;
		public TupleExtend(int it1,int it2):base(it1,it2)
		{
			
		}
		public TupleExtend((int,int)p) : base(p.Item1,p.Item2)
		{

		}
	}
}
