using System;


namespace Test
{
	public interface ITask
	{
		TaskPriorityEnum Priority { get; }

		void Start();
		ITask Subscribe(Action feedback);
		void Stop();
	}
}