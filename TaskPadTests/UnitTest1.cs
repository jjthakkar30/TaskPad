using TaskPad;

namespace TaskPadTests
{
	public class Tests
	{
		[SetUp]
		public void Setup() { }

		[Test]
		public void TestTaskIsAddedToMainList()
		{
			TaskItem TestTask = new("TestTitle", "TestDesc");
			int initialTaskCount = TodoManager.GetTaskCount();
			TodoManager.AddTask(TestTask);
			Assert.That(initialTaskCount + 1, Is.EqualTo(TodoManager.GetTaskCount()));
		}

		[Test]
		public void TestTaskListReturnTypeIsArray()
		{
			Assert.IsInstanceOf<TaskItem[]>(TodoManager.GetAllTasks());
		}

		[Test]
		public void TestTaskCountReturnTypeIsInt()
		{
			Assert.IsInstanceOf<int>(TodoManager.GetTaskCount());
		}

		[Test]
		public void TestSpecificTaskReturnType()
		{
			TaskItem TestTask = new("TestTitle", "TestDesc");
			TodoManager.AddTask(TestTask);
			Assert.IsInstanceOf<TaskItem>(TodoManager.GetSpecificTask(TestTask.ID));
		}

		[Test]
		public void TestTaskIsMarkedAsDone()
		{
			TaskItem TestTask = new("TestTitle", "TestDesc");
			TodoManager.AddTask(TestTask);
			TodoManager.MarkTaskAsDone(TestTask.ID);
			Assert.IsTrue(TestTask.IfCompleted);
		}

		[Test]
		public static void TestDeletingAllTask()
		{
			TodoManager.AddTask(new("TestTitle1", "TestDesc1"));
			TodoManager.AddTask(new("TestTitle2", "TestDesc2"));
			TodoManager.AddTask(new("TestTitle3", "TestDesc3"));
			TodoManager.DeleteAllTasks();
			Assert.That(TodoManager.GetTaskCount(), Is.EqualTo(0));
		}

		[Test]
		public void TestLoadedListReturnTypeIsSortedList()
		{
			Assert.IsInstanceOf<SortedList<int, TaskItem>>(FileHandler.LoadListFromFile());
		}

		[Test]
		public void TestUniqueIDReturnTypeAsInt()
		{
			Assert.IsInstanceOf<int>(TodoManager.GetUniqueID());
		}
	}
}