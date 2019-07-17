using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseInterfaces
{
	public static class StringFunctions
	{
		public static int[] ParseStringToArray(this string arr)
		{
			List<int> array = new List<int>();

			bool isParsed = false;

			string[] values = arr.Split(' ');

			foreach (var value in values)
			{
				int item;
				isParsed = int.TryParse(value, out item);

				if (isParsed)
					array.Add(item);
			}

			return array.ToArray();
		}

		public static string ParseArrayToString(this int[] array)
		{
			StringBuilder array_content = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (i != array.Length - 1)
					array_content.Append(array[i] + " ");
				else
					array_content.Append(array[i]);
			}

			return array_content.ToString();
		}
	}
}
