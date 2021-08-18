using UnityEngine;


namespace Test.IK
{
	public class Ragdoll : MonoBehaviour
	{
		public void Die()
		{
            //включается физика
			SetKinematic(false);
            //выключается аниматор
			GetComponent<Animator>().enabled = false;
		}

		private void Start()
		{
			SetKinematic(true);
		}

		private void SetKinematic(bool newValue)
		{
			var bodies = GetComponentsInChildren<Rigidbody>();
			foreach (var body in bodies)
			{
				body.isKinematic = newValue;
			}
		}
	}
}