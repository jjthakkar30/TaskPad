using System.Security.Cryptography.X509Certificates;

namespace TaskPad
{
	internal class Program
	{

		//Main implements the basic flow of the task pad app life cycle
		static void Main()
		{
			bool appFlag = true;
			LoadTasksFromFileProgram();

			while (appFlag)
			{
				Console.WriteLine("Welcome to TaskPad!\n");
				DisplayMenu();
				int userInput = ValidInput.GetIntFromUser("\nEnter your choice: ", 1, 9);

				switch (userInput)
				{
					case 1:
						AddTaskProgram();
						break;

					case 2:
						ViewAllTasksProgram();
						break;

					case 3:
						ViewSpecificTaskProgram();
						break;

					case 4:
						MarkTaskAsDoneProgram();
						break;

					case 5:
						UpdateTaskProgram();
						break;

					case 6:
						DeleteTaskProgram();
						break;

					case 7:
						DeleteAllTasksProgram();
						break;

					case 8:
						SaveTasksToFileProgram();
						break;

					case 9:
						TodoManager.SaveTasksToFile();
						appFlag = false;
						break;
				}
			}
		}


		//To seperate menu display code for repeated use, called in main.whileLoop
		public static void DisplayMenu()
		{
			Console.WriteLine("1. Add a task");
			Console.WriteLine("2. View all tasks");
			Console.WriteLine("3. View a Specific Task");
			Console.WriteLine("4. Mark a task as completed");
			Console.WriteLine("5. Update a task");
			Console.WriteLine("6. Delete a task");
			Console.WriteLine("7. Delete all tasks");
			Console.WriteLine("8. Save task list");
			Console.WriteLine("9. Exit TaskPad");
		}

		//To display a task (be it short or long)
		public static void DisplayTask(TaskItem _, bool all = false)
		{
			Console.WriteLine($"\n\nTask ID: {_.ID}");
			Console.WriteLine($"Title: {_.Title}");
			Console.WriteLine($"Description: {_.Desc}");
			Console.WriteLine($"Status:{(_.IfCompleted ? "" : " Not")} Completed");
			Console.WriteLine($"Due Date: {(_.TaskDueDate == DateTime.MaxValue ? "None added" : _.TaskDueDate.ToString())}");
			Console.WriteLine(@$"Priority: {(
				_.TaskPriority == Priority.Low ? "Low" :
				_.TaskPriority == Priority.Normal ? "Normal" : "High"
				)}");

			if(all)
			{
				Console.WriteLine($"Created On: {_.TaskCreationDate.ToString()}");
				Console.WriteLine($"Last Modified On: {_.TaskLastModifiedDate.ToString()}");
			}
			Console.WriteLine("\n");
		}

		//To check if action can be performed, depending on number of tasks in list.
		public static bool CheckIfPageLoadable()
		{
			bool flag = TodoManager.GetTaskCount() > 0;
			if (!flag) Console.WriteLine("\nCannot perform action. Task List is empty.\n");
			return flag;
		}


		//Functions defined to generate Validated Commands from user and call functions from the main interface

		//To add new taskItem to list using clean input
		public static void AddTaskProgram()
		{
			Console.Clear();
			Console.WriteLine("Add a New Task:\n\n");

			TaskItem newTaskItem = new();
			newTaskItem.IfCompleted = false;
			newTaskItem.ID = TodoManager.GetUniqueID();
			newTaskItem.Title = ValidInput.GetStringFromUser("Enter task title: ");
			newTaskItem.Desc = ValidInput.GetStringFromUser("Enter task description: ");
			newTaskItem.TaskCreationDate = DateTime.Now;
			newTaskItem.TaskLastModifiedDate = DateTime.Now;
			newTaskItem.TaskDueDate = ValidInput.GetDueDateFromUser();
			newTaskItem.TaskPriority = ValidInput.GetTaskPriorityFromUser();

			TodoManager.AddTask(newTaskItem);

			Console.WriteLine("\nTask Added Successfully\n\n");

			ShowReturnToMainMenu();
		}

		//To fetch Task List to main and display them + sort
		public static void ViewAllTasksProgram()
		{
			Console.Clear();

			if(CheckIfPageLoadable())
			{
				Console.Clear();
				TaskItem[] tiList = TodoManager.GetAllTasks(), displayList = tiList;
				while (true)
				{
					Console.Clear();
					Console.WriteLine("All Tasks:\n");

					Console.WriteLine("Press 1 to sort by Priority");
					Console.WriteLine("Press 2 to sort by Due Date");
					Console.WriteLine("Press 3 to sort by Completion Status");
					Console.WriteLine("Press 0 to remove sort selections\n");

					foreach (TaskItem ti in displayList)
					{
						DisplayTask(ti);
					}

					Console.WriteLine("\n\nPress Escape to return to Main Menu ");
					ConsoleKey inputKey = ConsoleKey.Escape;
					while (true)
					{
						inputKey = Console.ReadKey(true).Key;
						if (inputKey == ConsoleKey.D1 || inputKey == ConsoleKey.NumPad1)
						{
							displayList = (from ti in tiList
															orderby ti.TaskPriority descending
															select ti).ToArray();
							break;
						}
						else if (inputKey == ConsoleKey.D2 || inputKey == ConsoleKey.NumPad2)
						{
							displayList = (from ti in tiList
															orderby ti.TaskDueDate ascending
															select ti).ToArray();
							break;
						}
						else if (inputKey == ConsoleKey.D3 || inputKey == ConsoleKey.NumPad3)
						{
							///////////
							displayList = (from ti in tiList
															orderby ti.IfCompleted, ti.TaskDueDate ascending
															select ti).ToArray();
							break;
						}
						else if (inputKey == ConsoleKey.D0 || inputKey == ConsoleKey.NumPad0)
						{
							displayList = tiList;
							break;
						}
						else if (inputKey == ConsoleKey.Escape)
						{
							break;
						}
						else
						{
							continue;
						}
					}
					if (inputKey == ConsoleKey.Escape) break;
				}
				Console.Clear();
			} else ShowReturnToMainMenu();
		}

		//To fetch a specific task using validated ID to display
		public static void ViewSpecificTaskProgram()
		{
			Console.Clear();

			Console.WriteLine("View a Task:\n\n");
			if(CheckIfPageLoadable())
			{
				int id = ValidInput.GetIDFromUser();
				DisplayTask(TodoManager.GetSpecificTask(id), true);
				ShowReturnToMainMenu();
			}
		}

		//To mark a task as completed in the task list from main
		public static void MarkTaskAsDoneProgram()
		{
			Console.Clear();

			Console.WriteLine("Mark task completed:\n\n");

			if(CheckIfPageLoadable())
			{
				int id = ValidInput.GetIDFromUser();

				Console.WriteLine("\nCurrent Task Details:");
				DisplayTask(TodoManager.GetSpecificTask(id));

				bool ifMark = ValidInput.GetYesOrNoFromUser("Mark task as completed?");
				if (ifMark)
				{
					TodoManager.MarkTaskAsDone(id);
					Console.WriteLine("\nTask marked as completed");
				}
				else
				{
					Console.WriteLine("\nTask Updation aborted");
				}
				ShowReturnToMainMenu();
			}
		}

		//To update specific task fields with user validated input
		public static void UpdateTaskProgram()
		{
			Console.Clear();

			Console.WriteLine("Update Task:\n\n");

			if(CheckIfPageLoadable() )
			{
				int taskID = ValidInput.GetIDFromUser();
				TaskItem ti = TodoManager.GetSpecificTask(taskID);

				Console.WriteLine("Current Task Details:");
				DisplayTask(ti, true);

				bool updateFlag = true;
				while (updateFlag)
				{
					Console.WriteLine("\nSelect Value to update:\n");
					Console.WriteLine("1. Title");
					Console.WriteLine("2. Description");
					Console.WriteLine("3. Completion status");
					Console.WriteLine("4. Due Date");
					Console.WriteLine("5. Priority");
					Console.WriteLine("Enter any other key to exit this menu\n");

					int userChoice = ValidInput.GetIntFromUser("Enter choice: ", 1, 5);

					Console.WriteLine();

					string pre = "\nEnter new ";
					switch (userChoice)
					{
						case 1:
							ti.Title = ValidInput.GetStringFromUser(pre + "Title: ");
							break;

						case 2:
							ti.Desc = ValidInput.GetStringFromUser(pre + "Description: ");
							break;

						case 3:
							Console.WriteLine("Update Completion Status to:\n1. Not Completed\n2. Completed\n");
							int newTaskStatus = ValidInput.GetIntFromUser("Your choice: ", 1, 2) - 1;
							ti.IfCompleted = Convert.ToBoolean(newTaskStatus);
							ti.TaskPriority = Priority.Low;
							break;

						case 4:
							ti.TaskDueDate = ValidInput.GetDateFromUser(pre + "Due date: ");
							break;

						case 5:
							Console.WriteLine($"\nSet Priority:\n\n1.Low\n2.Normal\n3.High");
							ti.TaskPriority = new Priority[] { Priority.Low, Priority.Normal, Priority.High }[ValidInput.GetIntFromUser(pre + "Priority : ", 1, 3) - 2];
							break;

						default:
							updateFlag = false;
							break;
					}

					Console.WriteLine("Task field updated successfully.\n");

					bool ifEdit = ValidInput.GetYesOrNoFromUser("Edit other task fields?");
					if (!ifEdit) updateFlag = false;
				}

				ti.TaskLastModifiedDate = DateTime.Now;
				TodoManager.UpdateTask(ti);

				Console.WriteLine("\n\nTask Update completed. Updated Task Details:\n");
				DisplayTask(ti, true);
			}
			ShowReturnToMainMenu();
		}

		//To delete a task from task list
		public static void DeleteTaskProgram()
		{
			Console.Clear();

			Console.WriteLine("Delete a Task:\n\n");

			if (CheckIfPageLoadable())
			{
				int id = ValidInput.GetIDFromUser();

				Console.WriteLine("\nCurrent Task Details:");
				DisplayTask(TodoManager.GetSpecificTask(id));

				if (ValidInput.GetYesOrNoFromUser("Delete selected task?"))
				{
					TodoManager.DeleteTask(id);
					Console.WriteLine("\nTask deleted.");
				}
				else
				{
					Console.WriteLine("\nTask deletion aborted");
				}

				ShowReturnToMainMenu();
			}

			else ShowReturnToMainMenu();
		}

		//To delete All Tasks
		public static void DeleteAllTasksProgram()
		{
			Console.Clear();

			if (ValidInput.GetYesOrNoFromUser("Are you sure you want to delete all tasks?"))
			{
				TodoManager.DeleteAllTasks();
				Console.WriteLine("\nAll Tasks Deleted\n");
			}
			else
			{
				Console.WriteLine("Action Aborted\n");
			}
			ShowReturnToMainMenu();
		}

		//To save the list to a local file
		public static void SaveTasksToFileProgram()
		{
			Console.Clear();
			TodoManager.SaveTasksToFile();
			Console.WriteLine("Task List has been updated\n");

			ShowReturnToMainMenu();
		}

		//To load a list from a local file
		public static void LoadTasksFromFileProgram()
		{
			TodoManager.LoadTasksFromFile();
		}

		//Shows return option
		public static void ShowReturnToMainMenu()
		{
			Console.WriteLine("\n\nPress Escape to return to Main Menu ");
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

			Console.Clear();
		}
	}
}