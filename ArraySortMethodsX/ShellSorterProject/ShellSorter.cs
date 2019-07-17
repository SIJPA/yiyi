using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArraySortMethodsX.Sorters.SorterInterfaces;

namespace ShellSorterProject
{
	public class ShellSorter : ISorter
	{
		public void Sort(int[] array)
		{
			int j;
			int step = array.Length / 2;
			while (step > 0)
			{
				for (int i = 0; i < (array.Length - step); i++)
				{
					j = i;
					while ((j >= 0) && (array[j] > array[j + step]))
					{
						int tmp = array[j];
						array[j] = array[j + step];
						array[j + step] = tmp;
						j -= step;
					}
				}
				step = step / 2;
			}
		}
	}
}
