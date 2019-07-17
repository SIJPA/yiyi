using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

using ArraySortMethodsX.Sorters.SorterInterfaces;
using BubbleSorterProject;
using QuickSorterProject;
using ShellSorterProject;
using SelectionSorterProject;
using ArrayFileProject;
using ArrayConsoleProject;
using ArrayRandomProject;
using System.Threading;
using LoggerProject;
using System.Reflection;
using ArrayADOProject;
using Main.Properties;
using DatabaseInterfaces;
using System.Diagnostics;
using ArrayEFProject;
using System.Configuration;

namespace Main
{
	class Program
	{
		static List<Sorter> _sorters = new List<Sorter>();
		static int[] _array;
		static object _lockObj = new object();
		static SqlConnection _connection;
		static IDatabaseInsert _dbInsert;
		static List<Thread> _threads = new List<Thread>();

		static void Main(string[] args)
		{
			do
			{
				DisposeObjects();

				while (SelectSorter()) ;
				while (GetArrayData()) ;
				OutputCollection(_array, "Source array: ");

				foreach (var sorter in _sorters)
				{
					if (sorter.IsSelected)
					{
						Thread thread = new Thread(SortArray);
						thread.Start(sorter);
						_threads.Add(thread);
					}
				}

				foreach (var thread in _threads)
					thread.Join();

				CloseConnection();

				Console.WriteLine("Another array? Q - to exit");
			}
			while (Console.ReadKey().Key != ConsoleKey.Q);
		}

		static void DisposeObjects()
		{
			Console.Clear();
			_threads.Clear();
			_sorters.Clear();
			InitializeSorters();
			_connection = null;
			_dbInsert = null;
		}

		static void CloseConnection()
		{
			if (_connection != null)
				_connection.Close();
		}

		static void InitializeSorters()
		{
			_sorters.Add(new Sorter(new BubbleSorter()));
			_sorters.Add(new Sorter(new QuickSorter()));
			_sorters.Add(new Sorter(new SelectionSorter()));
			_sorters.Add(new Sorter(new ShellSorter()));
		}

		static bool SelectSorter()
		{
			Console.WriteLine("Add the sort methods: (1-5), 0 - quit");

			for (int i = 0; i < _sorters.Count; i++)
				Console.WriteLine(string.Format("{0}: ({1})", _sorters[i].Type.Name, i + 1));

			Console.WriteLine(string.Format("All sort methods: ({0})", _sorters.Count + 1));
			Console.WriteLine(string.Format("From DLL: ({0})", _sorters.Count + 2));

			int key = ParseInteger();
			if (key == 0)
				return false;

			if (key <= _sorters.Count)
				_sorters[key - 1].IsSelected = true;

			if (key == _sorters.Count + 1)
				_sorters.ForEach(x => x.IsSelected = true);

			if (key == _sorters.Count + 2)
			{
				string filePath;
				while (!FileIsExist(out filePath)) ;

				Assembly dllAssembly = Assembly.LoadFrom(filePath);
				var iSorterTypes = dllAssembly.GetTypes().Where(x => x.GetInterface("ISorter") != null).ToList();

				foreach (var iSorterType in iSorterTypes)
				{
					ConstructorInfo ci = iSorterType.GetConstructor(new Type[] { });
					ISorter iSorterObject = ci.Invoke(new object[] { }) as ISorter;
					Sorter sorter = new Sorter(iSorterObject);
					sorter.IsSelected = true;
					_sorters.Add(sorter);
				}
			}

			return true;
		}

		static bool GetArrayData()
		{
			Console.WriteLine("Loading data from (1-5).");
			Console.WriteLine("File: (1)");
			Console.WriteLine("Console: (2)");
			Console.WriteLine("Random array: (3)");
			Console.WriteLine("Database (ADO.NET): (4)");
			Console.WriteLine("Database (EF): (5)");

			int key = ParseInteger();

			switch (key)
			{
				case 0:
					return false;
				case 1:
					{
						string filePath;
						while (!FileIsExist(out filePath)) ;
						_array = ArrayFile.GetArray(filePath);
						break;
					}
				case 2:
					{
						Console.WriteLine("Enter the integer values of the array");
						Console.WriteLine("To finish, type any letter");

						_array = ArrayConsole.GetArray();
						break;
					}
				case 3:
					{
						Console.WriteLine("Enter count of numbers: ");
						int numCount = ParseInteger();
						Console.WriteLine("Enter minimum number: ");
						int min = ParseInteger();
						Console.WriteLine("Enter maximum number: ");
						int max = ParseInteger();

						_array = ArrayRandom.GetArray(numCount, min, max);
						break;
					}
				case 4:
					{
						Console.WriteLine("Enter array ID: ");
						int id = ParseInteger();

						_connection = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString"));
						ArrayADO ado = new ArrayADO(_connection);
						_array = ado.GetArray(id);

						_dbInsert = ado;
						_dbInsert.ArrayId = id;
						break;
					}
				case 5:
					{
						Console.WriteLine("Enter array ID: ");
						int id = ParseInteger();

						ArrayEF ef = new ArrayEF(ConfigurationManager.AppSettings.Get("ConnectionString"));
						_array = ef.GetArray(id);

						_dbInsert = ef;
						_dbInsert.ArrayId = id;
						break;
					}
			}

			return false;
		}

		static void SortArray(object sorter)
		{
			Sorter sortObj = sorter as Sorter;
			int[] _resultArray = (int[])_array.Clone();

			Stopwatch watcher = new Stopwatch();
			watcher.Start();
			sortObj.Sort(_resultArray);
			watcher.Stop();

			lock (_lockObj)
			{
				OutputCollection(_resultArray, string.Format("Method: {0}", sortObj.Type.Name));
				Logger.Write(MessageType.Info, string.Format("Sorted an array by {0}", sortObj.Type.Name));

				if (_dbInsert != null)
					_dbInsert.Insert((float)watcher.Elapsed.TotalMilliseconds, sortObj.Type.Name, _resultArray);
			}
		}

		static void OutputCollection(IEnumerable<int> collection, string message)
		{
			Console.WriteLine(message);

			foreach (var value in collection)
				Console.Write(value + " ");

			Console.WriteLine();
		}

		static bool FileIsExist(out string filePath)
		{
			Console.WriteLine("Enter file path: ");
			bool isExist = File.Exists(filePath = Console.ReadLine());

			if (isExist)
				return true;
			else
			{
				Console.WriteLine("File not exist!");
				Logger.Write(MessageType.Error, string.Format("File {0} not exist!", filePath));
				return false;
			}
		}

		static int ParseInteger()
		{
			try
			{
				return int.Parse(Console.ReadLine());
			}
			catch
			{
				Console.WriteLine("Invalid value. Please try again");
				Logger.Write(MessageType.Warning, "Error parsing integer value");
				return ParseInteger();
			}
		}
	}
}
