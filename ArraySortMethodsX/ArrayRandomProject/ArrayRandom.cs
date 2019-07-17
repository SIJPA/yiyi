using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayRandomProject
{
	public static class ArrayRandom
	{
		public static int[] GetArray(int numCount, int min, int max)
		{
			Random random = new Random();
			List<int> array = new List<int>();
			for (int i = 0; i < numCount; i++)
				array.Add(random.Next(min, max));

			return array.ToArray();
		}
	}
}