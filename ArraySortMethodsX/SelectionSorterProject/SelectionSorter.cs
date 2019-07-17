using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArraySortMethodsX.Sorters.SorterInterfaces;

namespace SelectionSorterProject
{
	public class SelectionSorter : ISorter
	{
		public void Sort(int[] array)
		{
			int min, temp;
			int length = array.Length;

			for (int i = 0; i < length - 1; i++)
			{
				min = i;

				for (int j = i + 1; j < length; j++)
				{
					if (array[j] < array[min])
					{
						min = j;
					}
				}

				if (min != i)
				{
					temp = array[i];
					array[i] = array[min];
					array[min] = temp;
				}
			}
		}
	}
}
