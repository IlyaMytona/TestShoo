using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Test.Helper
{
    public static partial class Extensions
    {
        public static T RandomObject<T>(this IList<T> list) 
	    {
		    return (list.Count > 0) ? list[Random.Range(0, list.Count)] : default(T);
        }

        public static float CalculateDistance(this Vector3 from, Vector3 to)
        {
            var distanceX = to.x - from.x;
            var distanceY = to.y - from.y;
            var distanceZ = to.z - from.z;

            return distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ;
        }

        public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
        {
            var delta = eulerAngle;

            if (delta.x > 180) delta.x -= 360;
            else if (delta.x < -180) delta.x += 360;

            if (delta.y > 180) delta.y -= 360;
            else if (delta.y < -180) delta.y += 360;

            if (delta.z > 180) delta.z -= 360;
            else if (delta.z < -180) delta.z += 360;
            //round values to angle;
            return new Vector3(delta.x, delta.y, delta.z);
        }

        public static void ApplyDamage(this GameObject receiver, InfoCollision info)
        {
            var receivers = receiver.GetComponents<ISetDamage>();
            if (receivers != null)
                for (int i = 0; i < receivers.Length; i++)
                    receivers[i].SetDamage(info);
        }
    }
}
