using ArraySortMethodsX.Sorters.SorterInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickSorterProject
{
	public class QuickSorter : ISorter
	{
		public void Sort(int[] array)
		{
			QuickSort(array, 0, array.Length - 1, Comparer<int>.Default);
		}

		static void SwapIfGreaterWithItems(int[] keys, IComparer<int> comparer, int a, int b)
		{
			if (a == b || comparer.Compare(keys[a], keys[b]) <= 0)
				return;
			int obj = keys[a];
			keys[a] = keys[b];
			keys[b] = obj;
		}

		static void QuickSort(int[] array, int left, int right, IComparer<int> comparer)
		{
			do
			{
				int index1 = left;
				int index2 = right;
				int index3 = index1 + (index2 - index1 >> 1);
				SwapIfGreaterWithItems(array, comparer, index1, index3);
				SwapIfGreaterWithItems(array, comparer, index1, index2);
				SwapIfGreaterWithItems(array, comparer, index3, index2);
				int obj1 = array[index3];
				do
				{
					while (comparer.Compare(array[index1], obj1) < 0)
						++index1;
					while (comparer.Compare(obj1, array[index2]) < 0)
						--index2;
					if (index1 <= index2)
					{
						if (index1 < index2)
						{
							int obj2 = array[index1];
							array[index1] = array[index2];
							array[index2] = obj2;
						}
						++index1;
						--index2;
					}
					else
						break;
				}
				while (index1 <= index2);
				if (index2 - left <= right - index1)
				{
					if (left < index2)
						QuickSort(array, left, index2, comparer);
					left = index1;
				}
				else
				{
					if (index1 < right)
						QuickSort(array, index1, right, comparer);
					right = index2;
				}
			}
			while (left < right);
		}
	}
}
