using System.Linq;
using UnityEngine;
using UnityEngine.AI;


namespace Test.Model.Ai
{
    public class Patrol
    {
        private static Vector3[] _listPoint;
        private static int _indexCurPoint;
        private static int _minDistance = 15;
        private static int _maxDistance = 80;

        static Patrol()
        {
            var tempPoints = Object.FindObjectsOfType<DestroyPoint>();
            _listPoint = tempPoints.Select(o => o.transform.position).ToArray();
        }

        public static Vector3 GenerateRandomPoint(Transform agent, bool isRandom = true)
		{
			Vector3 result;
            if (isRandom)
            {
                var dis = Random.Range(_minDistance, _maxDistance);
                var randomPoint = Random.insideUnitSphere * dis;
                NavMesh.SamplePosition(agent.position + randomPoint, out var hit, dis, NavMesh.AllAreas);
                result = hit.position;
            }

            else
            {
                if (_indexCurPoint < _listPoint.Length - 1)
                {
                    _indexCurPoint++;
                }
                else
                {                    
                    _indexCurPoint = 0;
                }
                result = _listPoint[_indexCurPoint];
            }
            return result;
		}
	}
}