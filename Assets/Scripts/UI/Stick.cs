using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

namespace Test.UI
{
    public class Stick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public enum AxisOption
        {
            Both,
            OnlyHorizontal,
            OnlyVertical
        }

        public int MovementRange = 100;
        public AxisOption axesToUse = AxisOption.Both;
        public string horizontalAxisName = "Horizontal";
        public string verticalAxisName = "Vertical";

        private Vector3 _startPosition;
        private bool _useXaxis;
        private bool _useYaxis;
        private CrossPlatformInputManager.VirtualAxis _horizontalVirtualAxis;
        private CrossPlatformInputManager.VirtualAxis _verticalVirtualAxis;

        private IEnumerator Start()
        {
            _startPosition = transform.position;
            yield return new WaitForEndOfFrame();

            CreateVirtualAxes();
        }

        private void UpdateVirtualAxes(Vector3 value)
        {
            var delta = _startPosition - value;
            delta.y = -delta.y;
            delta /= MovementRange;
            if (_useXaxis)
            {
                _horizontalVirtualAxis.Update(-delta.x);
            }

            if (_useYaxis)
            {
                _verticalVirtualAxis.Update(delta.y);
            }
        }

        private void CreateVirtualAxes()
        {
            _useXaxis = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            _useYaxis = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (_useXaxis)
            {
                if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
                {
                    _horizontalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(horizontalAxisName);
                }
                else
                {
                    _horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                    CrossPlatformInputManager.RegisterVirtualAxis(_horizontalVirtualAxis);
                }
            }
            if (_useYaxis)
            {
                if (CrossPlatformInputManager.AxisExists(verticalAxisName))
                {
                    _verticalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(verticalAxisName);
                }
                else
                {
                    _verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                    CrossPlatformInputManager.RegisterVirtualAxis(_verticalVirtualAxis);
                }
            }
        }

        public void OnDrag(PointerEventData data)
        {
            Vector3 newPos = Vector3.zero;

            if (_useXaxis)
            {
                int delta = (int)(data.position.x - _startPosition.x);
                newPos.x = delta;
            }

            if (_useYaxis)
            {
                int delta = (int)(data.position.y - _startPosition.y);
                newPos.y = delta;
            }
            // change to clamp on a circular area instead of a square area
            transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + _startPosition;
            UpdateVirtualAxes(transform.position);
        }

        public void OnPointerUp(PointerEventData data)
        {
            transform.position = _startPosition;
            UpdateVirtualAxes(_startPosition);
        }

        public void OnPointerDown(PointerEventData data)
        {

        }
    }
}
