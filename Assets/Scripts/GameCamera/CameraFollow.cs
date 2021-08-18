using UnityEngine;


namespace Test.GameCamera
{
    public class CameraFollow : MonoBehaviour
    {
        #region Fields

        private GameObject _target;
        public float damping = 0.5f;
        public Vector3 offset;

        #endregion


        #region Unity Methods

        private void LateUpdate()
        {
            if (_target == null)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
            }
            else //if (target != null)
            {
                Vector3 desiredPosition = _target.transform.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, damping);
                transform.position = smoothedPosition;

                transform.LookAt(_target.transform);
            }
        }

        #endregion
    }
}
