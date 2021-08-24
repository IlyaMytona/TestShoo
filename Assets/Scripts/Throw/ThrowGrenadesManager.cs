using System.Collections;
using System.Collections.Generic;
using Test.GameServices;
using Test.Helper;
using UnityEngine;


namespace Test.Throw
{
    [RequireComponent(typeof(LineRenderer))]    
    public class ThrowGrenadesManager : MonoBehaviour
    {
        private const float _directionY = 0.0f;
        private const float _rotationSpeed = 10.0f;
        #region Private Data

        [SerializeField] private Transform _throwStartPoint;
        [SerializeField] private GameObject _throwEnd;
        [SerializeField] private Rigidbody _grenade;
        [SerializeField] private LayerMask _obstacles = 1 << 0;

        private LineRenderer _lineRenderer;
        private Camera _cameraMain;        
        [Tooltip("affects the maximum range of the grenade")]
        private float _throwMaxForce = 15.0f;
        [Tooltip("Delay to exit the Throw Aiming Mode and get back to default motion")]
        private float _exitThrowModeDelay = 0.5f;
        private float _throwDelayTime = 0.25f;
        private float _timestepForDrawingTrajectory = 0.1f;
        private float _maximumTimeFoDrawTrajectory = 10.0f;
        private int _countsOfGrenades = 6;        

        #endregion


        #region Fields

        public bool IsAiming;        
        [HideInInspector] public bool InThrow;
        [HideInInspector] public bool IsThrowInput;
        [HideInInspector] public bool IsInitialize;

        #endregion        


        public virtual Vector3 AimDirection
        {
            get
            {
                return AimPoint - new Vector3(transform.position.x, 1.4f, transform.position.z);
            }
        }

        public virtual Vector3 AimPoint
        {
            get
            {
                var position = MousePositionHandler.Instance.WorldMousePosition(_obstacles);
                position.y = transform.position.y;
                return position;
            }
        }

        private Vector3 StartVelocity
        {
            get
            {
                var distance = transform.position.CalculateDistance(AimPoint);
                var force = Mathf.Clamp(distance, 0, _throwMaxForce);
                var rotation = Quaternion.LookRotation(AimDirection.normalized, Vector3.up);
                var direction = Quaternion.AngleAxis(rotation.eulerAngles.NormalizeAngle().x, transform.right) * transform.forward;
                return direction * force;
            }
        }        

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _cameraMain = Camera.main;
            _lineRenderer = GetComponent<LineRenderer>();
            if (_lineRenderer) _lineRenderer.useWorldSpace = true;
            Services.Instance.LevelService.ThrowGrenadesManager = this;
            IsInitialize = true;
        }
        
        public void RotationToSpawnGrenadeZone()
        {
            if (IsAiming)
            {
                var direction = AimDirection;
                direction.y = _directionY;                
                float rotationSpeed = _rotationSpeed;
                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, direction.normalized, rotationSpeed * Time.deltaTime, 0.1f);
                Quaternion _newRotation = Quaternion.LookRotation(desiredForward); 
                transform.rotation = _newRotation;
            }
        }        
        
        private void FixedUpdate()
        {
            if (_grenade == null)
            {
                IsAiming = false;
                InThrow = false;
                IsThrowInput = false;
                if (_lineRenderer && _lineRenderer.enabled) _lineRenderer.enabled = false;
                if (_throwEnd && _throwEnd.activeSelf) _throwEnd.SetActive(false);
                return;
            }

            if (IsAiming)
                DrawTrajectory();
            else
            {
                if (_lineRenderer && _lineRenderer.enabled) _lineRenderer.enabled = false;
                if (_throwEnd && _throwEnd.activeSelf) _throwEnd.SetActive(false);
            }

            if (IsThrowInput)
            {
                InThrow = true;
                IsThrowInput = false;
                _countsOfGrenades -= 1;
                StartCoroutine(Launch());
            }
        }

        private void DrawTrajectory()
        {
            var points = GetTrajectoryPoints(_throwStartPoint.position, StartVelocity, _timestepForDrawingTrajectory, _maximumTimeFoDrawTrajectory);
            if (_lineRenderer)
            {
                if (!_lineRenderer.enabled) _lineRenderer.enabled = true;
                _lineRenderer.positionCount = points.Count;
                _lineRenderer.SetPositions(points.ToArray());
            }
            if (_throwEnd)
            {
                if (!_throwEnd.activeSelf) _throwEnd.SetActive(true);
                if (points.Count > 1)
                    _throwEnd.transform.position = points[points.Count - 1];
            }
        }

        private IEnumerator Launch()
        {
            yield return new WaitForSeconds(_throwDelayTime);
            var grenadeObject = Instantiate(_grenade, _throwStartPoint.position, _throwStartPoint.rotation) as Rigidbody;
            grenadeObject.isKinematic = false;
            LaunchObject(grenadeObject);
            yield return new WaitForSeconds(2 * _timestepForDrawingTrajectory);

            var collider = grenadeObject.GetComponent<Collider>();
            if (collider)
                collider.isTrigger = false;

            InThrow = false;

            if (_countsOfGrenades <= 0)
                _grenade = null;
            yield return new WaitForSeconds(_exitThrowModeDelay);

            PrepareToThrow(false);
        }

        public void PrepareToThrow(bool value)
        {
            IsAiming = value;
        }

        private void LaunchObject(Rigidbody projectily)
        {
            projectily.AddForce(StartVelocity, ForceMode.VelocityChange);
        }        
        
        private List<Vector3> GetTrajectoryPoints(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
        {
            Vector3 previous = start;
            List<Vector3> points = new List<Vector3>();
            points.Add(previous);
            for (int i = 1; ; i++)
            {
                float time = timestep * i;
                if (time > maxTime) break;
                Vector3 position = PlotTrajectoryAtTime(start, startVelocity, time);
                RaycastHit hit;
                if (Physics.Linecast(previous, position, out hit, _obstacles))
                {
                    points.Add(hit.point);
                    break;
                }
                points.Add(position);
                previous = position;
            }
            return points;
        }

        private Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
        {
            return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
        }
    }
}