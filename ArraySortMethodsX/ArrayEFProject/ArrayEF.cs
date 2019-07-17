using ArrayEFProject.Models;
using DatabaseInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ArrayEFProject
{
	public class ArrayEF : IDatabaseInsert
	{
		public int ArrayId { get; set; }
		Entities _dbModel;

		public ArrayEF(string connectionString)
		{
			_dbModel = new Entities(connectionString);
		}

		public int[] GetArray(int id)
		{
			var arrayString = _dbModel.input_arrays.Where(x => x.array_id == id).Select(x => x.array_content).Single();
			return arrayString.ParseStringToArray();
		}

		public void Insert(float sortingTime, string sorterName, int[] array)
		{
			_dbModel.sorted_arrays.Add(new sorted_arrays()
				{
					array_id = ArrayId,
					sorting_time = sortingTime,
					sorter_name = sorterName,
					array_content = array.ParseArrayToString()
				});

			_dbModel.SaveChanges();
		}
	}
}