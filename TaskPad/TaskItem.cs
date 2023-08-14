using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPad
{
	public enum Priority : int
	{
		Low = -1,
		Normal = 0,
		High = 1
	};

	public class TaskItem
	{
		public int ID { get; set; }
		public string? Title { get; set; }
		public string? Desc{ get; set; }
		public bool IfCompleted { get; set; }

		public Priority? TaskPriority { get; set; } = Priority.Normal;
		public DateTime? TaskDueDate { get; set; }
		public DateTime? TaskCreationDate { get; set; }
		public DateTime? TaskLastModifiedDate { get; set; }

		public TaskItem() { }

		public TaskItem(string title, string desc)
		{
			ID = TodoManager.GetUniqueID();
			Title = title;
			Desc = desc;
			this.IfCompleted = false;

			TaskPriority = Priority.Normal;
			TaskDueDate = DateTime.MaxValue;
			TaskCreationDate = DateTime.Now;
			TaskLastModifiedDate = DateTime.Now;
		}
	}
}