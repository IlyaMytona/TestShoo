using UnityEngine;

namespace Test.UI
{
    public class PlayerDisplay : MonoBehaviour
    {
        #region Private Data

        private Transform _cameraTransform;
        private Transform _сanvasDisplayTransform;
        private float _size;

        #endregion


        #region Fields

        public bool ScaleWithDistance = false;
        public float ScaleMultiplier = 1.0f;

        #endregion


        #region Unity Methods

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            _сanvasDisplayTransform = transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(_сanvasDisplayTransform.position + _cameraTransform.rotation * Vector3.forward, _cameraTransform.rotation * Vector3.up);
            if (!ScaleWithDistance) return;
            _size = (_cameraTransform.position - transform.position).magnitude;
            transform.localScale = Vector3.one * (_size * (ScaleMultiplier / 100f));
        }

        #endregion
    }
}
