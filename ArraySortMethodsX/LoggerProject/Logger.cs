using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoggerProject
{
	public static class Logger
	{
		static string _filePath = "log.txt";

		public static void Write(MessageType type, string message)
		{
			using (StreamWriter sw = File.AppendText(_filePath))
				sw.WriteLine(string.Format("[{0}] - DateTime: {1} - {2}", type.ToString(), DateTime.Now, message));
		}
	}
}
