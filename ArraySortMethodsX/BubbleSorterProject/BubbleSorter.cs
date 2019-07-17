using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArraySortMethodsX.Sorters.SorterInterfaces;

namespace BubbleSorterProject
{
	public class BubbleSorter : ISorter
	{
		public void Sort(int[] array)
		{
			int temp = 0;
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = i + 1; j < array.Length; j++)
				{
					if (array[i] > array[j])
					{
						temp = array[i];
						array[i] = array[j];
						array[j] = temp;
					}
				}
			}
		}
	}
}
