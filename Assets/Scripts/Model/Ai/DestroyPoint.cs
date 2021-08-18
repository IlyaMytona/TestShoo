using System;
using UnityEngine;


namespace Test.Model.Ai
{
	public class DestroyPoint : MonoBehaviour
	{
		public event Action<GameObject> OnFinishChange;
        //как только с точкой будет сталкиватся объект с компонентом <Bot>
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Bot>())
			{
                //будет вызыватся событие передачи gameObject
                OnFinishChange?.Invoke(gameObject);
			}
		}
	}
}