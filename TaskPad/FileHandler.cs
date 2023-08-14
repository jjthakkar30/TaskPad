using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskPad
{
	public static class FileHandler
	{
		//Main DB file path
		private static readonly string fileDBPath = @"C:\Users\JinilThakkar\OneDrive - Kongsberg Digital AS\Desktop\Training\00 - Labs\00 - Module Projects\C# Console App\TaskPad\TaskPad\bin\Debug\net6.0\TaskList.json";

		public static void SaveListToFile(SortedList<int, TaskItem> taskList)
		{
			File.WriteAllText(fileDBPath, JsonSerializer.Serialize(taskList));
		}

		public static SortedList<int, TaskItem> LoadListFromFile()
		{
			string jsonList = File.ReadAllText(fileDBPath);

			if (jsonList.Length == 0) return new SortedList<int, TaskItem>();

			return JsonSerializer.Deserialize<SortedList<int, TaskItem>>(jsonList);
		}
	}
}
