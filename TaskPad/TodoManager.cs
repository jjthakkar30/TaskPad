using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TaskPad
{
	static public class TodoManager
	{
		//Main Task List
		private static SortedList<int, TaskItem> taskList = new();

		//adding a task in main list
		public static void AddTask(TaskItem ti)
		{
			taskList.Add(ti.ID, ti);
		}

		// return tasks array
		public static TaskItem[] GetAllTasks() {
			return taskList.Values.ToArray();
		}

		// return total task count
		public static int GetTaskCount()
		{
			return taskList.Count;
		}

		//return a single specific task
		public static TaskItem GetSpecificTask(int id)
		{
				return taskList[id];
		}

		//marks a task in list as completed
		public static void MarkTaskAsDone(int id)
		{
			taskList[id].IfCompleted = true;
			taskList[id].TaskLastModifiedDate = DateTime.Now;
			taskList[id].TaskPriority = Priority.Low;
		}

		// recieves a new task, and updates it by id.
		public static void UpdateTask(TaskItem ti)
		{
			taskList[ti.ID] = ti;
		}

		//removes a task from list by id
		public static void DeleteTask(int id)
		{
			taskList.Remove(id);
		}

		//deletes all task from tasklist
		public static void DeleteAllTasks()
		{
			taskList = new();
			SaveTasksToFile();
		}

		//saves current list to file
		public static void SaveTasksToFile()
		{
			FileHandler.SaveListToFile(taskList);
		}

		//loads task from file
		public static void LoadTasksFromFile()
		{
			taskList = FileHandler.LoadListFromFile();
		}

		//checks if entered id is valid
		public static bool IsValidKey(int id)
		{
			return taskList.ContainsKey(id);
		}

		//returns a new unique id for a new task
		public static int GetUniqueID()
		{
			if (taskList.Count == 0) return 1;

			int uniqID = -1;
			foreach (int k in taskList.Keys)
			{
				uniqID = k+1 > uniqID ? k+1 : uniqID;
			}

			return uniqID;
		}
	}
}
