using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPad
{
	//Static utility functions defined in this static class for getting Valid Input from User
	static internal class ValidInput
	{
		//To Get Valid Integer input from user between a integer range using string inputPrompt
		public static int GetIntFromUser(string inputPrompt, int lowerLimit, int upperLimit)
		{
			while (true)
			{
				Console.Write(inputPrompt);
				if (int.TryParse(Console.ReadLine(), out int userInput) && userInput >= lowerLimit && userInput <= upperLimit)
				{
					return userInput;
				}
				else
				{
					Console.WriteLine("Invalid Input. Please try again.\n");
				}
			}
		}

		//To Get Valid Non-Empty String input from user using string inputPrompt
		public static string GetStringFromUser(string inputPrompt)
		{
			while (true)
			{
				Console.Write(inputPrompt);
				string? inputString = Console.ReadLine();
				if (inputString != "") inputString = inputString.Trim();
				if (string.IsNullOrEmpty(inputString))
				{
					Console.WriteLine("Invalid Input. Please try again.\n");
				}
				else
				{
					return inputString;
				}
			}
		}

		//To Get Valid ID Input from user for Task Manipulation
		public static int GetIDFromUser()
		{
			int inputID;
			while (true)
			{
				inputID = GetIntFromUser("Enter an ID: ", 1, TodoManager.GetUniqueID() - 1);
				if (TodoManager.IsValidKey(inputID)) break;
				else Console.WriteLine("Invalid ID entered. Please try again.\n");
			}
			return inputID;
		}

		//To get a yes or a no from User
		public static bool GetYesOrNoFromUser(string userPrompt)
		{
			ConsoleKey inputKey;
			while (true)
			{
				Console.Write(userPrompt + " (Y/n)  ");
				inputKey = Console.ReadKey().Key;
				if (inputKey == ConsoleKey.Y || inputKey == ConsoleKey.N) break;
				else Console.WriteLine("\nInvalid Input. Please try again.\n\n");
			}
			return inputKey == ConsoleKey.Y;
		}

		//To get a vaild Date Input from User with min value now
		public static DateTime GetDateFromUser(string userPrompt)
		{
			while (true)
			{
				Console.Write(userPrompt);
				if (DateTime.TryParse(Console.ReadLine(), out DateTime DueDate) && DueDate > DateTime.Now)
				{
					return DueDate;
				}
				else
				{
					Console.WriteLine("Invalid date input. Please try again.\n");
				}
			}
		}

		//To get a valid Due Date Input from User if needed else returns DateTime.MaxValue
		public static DateTime GetDueDateFromUser()
		{
			return GetYesOrNoFromUser("\nDo you want to enter a Due Date ?") ? GetDateFromUser("\nEnter task due date (DD/MM/YYYY): ") : DateTime.MaxValue ;
		}

		//To get Valid Task Priority Input from User
		public static Priority GetTaskPriorityFromUser()
		{
			Priority taskPriority = Priority.Normal;
			if(GetYesOrNoFromUser("\nDo you want to enter task priority ?"))
			{
				Console.WriteLine($"\nSet Priority:\n\n1.Low\n2.Normal\n3.High");

				//taskPriority =  new Priority[]{ Priority.Low, Priority.Normal, Priority.High }[ GetIntFromUser("\nyour priority choice: ", 1, 3) - 2 ];

				int userChoice = GetIntFromUser("\nyour priority choice: ", 1, 3);
				taskPriority = userChoice == 1 ? Priority.Low : userChoice == 2 ? Priority.Normal : Priority.High; 
			}
			return taskPriority;
		}
	}
}
