using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseInterfaces
{
	public interface IDatabaseInsert
	{
		int ArrayId { get; set; }
		void Insert(float sortingTime, string sorterName, int[] array);
	}
}