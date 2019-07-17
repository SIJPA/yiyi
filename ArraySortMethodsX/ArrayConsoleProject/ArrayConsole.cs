using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayConsoleProject
{
	public static class ArrayConsole
	{
		public static int[] GetArray()
		{
			List<int> array = new List<int>();

			bool isParsed = false;

			do
			{
				string line = Console.ReadLine();
				string[] values = line.Split(' ');

				foreach (var value in values)
				{
					int item;
					isParsed = int.TryParse(value, out item);

					if (isParsed)
						array.Add(item);
				}
			}
			while (isParsed);

			return array.ToArray();
		}
	}
}
