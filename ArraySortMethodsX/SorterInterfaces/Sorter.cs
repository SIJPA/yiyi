using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArraySortMethodsX.Sorters.SorterInterfaces
{
	public class Sorter
	{
		ISorter _sorter;

		public bool IsSelected { get; set; }
		public Type Type
		{
			get
			{
				return _sorter.GetType();
			}
		}

		public Sorter(ISorter sorter)
		{
			_sorter = sorter;
			IsSelected = false;
		}

		public void Sort(object array)
		{
			_sorter.Sort((int[])array);
		}
	}
}
