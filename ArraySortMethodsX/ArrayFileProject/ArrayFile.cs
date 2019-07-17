using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ArrayFileProject
{
	public static class ArrayFile
	{
		public static int[] GetArray(string filePath)
		{
			int[] array = null;

			try
			{
				array = File.ReadAllText(filePath).Split(' ').Select(n => int.Parse(n)).ToArray();
			}
			catch (FormatException)
			{
				Console.WriteLine("Bad format data");
			}

			return array;
		}
	}
}
