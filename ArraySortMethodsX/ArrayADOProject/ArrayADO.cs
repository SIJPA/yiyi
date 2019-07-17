using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using DatabaseInterfaces;

namespace ArrayADOProject
{
	public class ArrayADO : IDatabaseInsert
	{
		SqlConnection _connection;
		public int ArrayId { get; set; }

		public ArrayADO(SqlConnection connection)
		{
			_connection = connection;
		}

		public int[] GetArray(int id)
		{
			SqlCommand com = new SqlCommand("SELECT [array_content] FROM [input_arrays] WHERE [array_id] = @id", _connection);
			com.Parameters.Add("@id", SqlDbType.Int);
			com.Parameters["@id"].Value = id;
			_connection.Open();
			SqlDataReader reader = com.ExecuteReader();

			reader.Read();
			string arr = (string)reader[0];
			reader.Close();

			return arr.ParseStringToArray();
		}

		public void Insert(float sortingTime, string sorterName, int[] array)
		{
			string sql = string.Format("Insert Into [sorted_arrays]" +
				   "(array_Id, sorting_time, sorter_name, array_content) Values(@arrayId, @sortingTime, @sorterName, @arrayContent)");

			using (SqlCommand cmd = new SqlCommand(sql, _connection))
			{
				cmd.Parameters.AddWithValue("@arrayId", ArrayId);
				cmd.Parameters.AddWithValue("@sortingTime", sortingTime);
				cmd.Parameters.AddWithValue("@sorterName", sorterName);
				cmd.Parameters.AddWithValue("@arrayContent", array.ParseArrayToString());

				cmd.ExecuteNonQuery();
			}
		}
	}
}
