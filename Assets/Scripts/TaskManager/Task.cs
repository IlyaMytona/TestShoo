using System;
using System.Collections;
using UnityEngine;


namespace Test
{
	public class Task : ITask
	{
		public TaskPriorityEnum Priority
		{
			get
			{
				return _taskPriority;
			}
		}

		private TaskPriorityEnum _taskPriority = TaskPriorityEnum.Default;

		private Action _feedback;
		private MonoBehaviour _coroutineHost;
		private Coroutine _coroutine;
		private IEnumerator _taskAction;

		public static Task Create(IEnumerator taskAction, MonoBehaviour coroutineHost, TaskPriorityEnum priority = TaskPriorityEnum.Default)
		{
			return new Task(taskAction, coroutineHost, priority);
		}

		public Task(IEnumerator taskAction, MonoBehaviour coroutineHost, TaskPriorityEnum priority = TaskPriorityEnum.Default)
		{
			_coroutineHost = coroutineHost;
			_taskPriority = priority;
			_taskAction = taskAction;
		}

		public void Start()
		{
			if (_coroutine == null)
			{
				_coroutine = _coroutineHost.StartCoroutine(RunTask());
			}
		}

		public void Stop()
		{
			if (_coroutine != null)
			{
				if (_coroutineHost != null) _coroutineHost.StopCoroutine(_coroutine);
				_coroutine = null;
			}
		}

		public ITask Subscribe(Action feedback)
		{
			_feedback += feedback;

			return this;
		}


		private IEnumerator RunTask()
		{
			yield return _taskAction;

			CallSubscribe();
		}

		private void CallSubscribe()
		{
			_feedback?.Invoke();
		}
	}
}