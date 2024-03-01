using Network;
using UnityEngine;

namespace Game
{
    public class FpCamera : NetworkClass
    {
        [Header("References")] [SerializeField]
        private Movement movement;
        [SerializeField] private Transform camHolder;
        [SerializeField] private Transform body;
        [SerializeField] private Camera cam;
    
        [Space(10)]
        [Header("Camera")]
        [SerializeField] private float sensitivity = 3f;
        [SerializeField] private float yAngleClamp = 90f;
        [SerializeField] private float defaultFOV = 60f;

        [Space(10)] 
        [Header("Bobbing")]
        [SerializeField] private float bobbingSpeed = 14f;
        [SerializeField] private float bobbingAmount = 0.05f;

        private float _xRotation;
        private float _timer;
        private float _midpoint;

        private void Start()
        {
            if (!IsMine)
            {
                camHolder.gameObject.SetActive(false);
                cam.enabled = false;
                enabled = false;
                return;
            }
            if (!movement) movement = GetComponent<Movement>();
            cam.fieldOfView = defaultFOV;
            _midpoint = camHolder.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            
        }
    

        private void Look(Vector2 lookInput)
        {
            if (!IsMine) return;
            float mouseX = lookInput.x * sensitivity * Time.deltaTime;
            float mouseY = lookInput.y * sensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -yAngleClamp, yAngleClamp);
        
            camHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            body.Rotate(Vector3.up * mouseX);
        }

        private void Update()
        {
            if (!IsMine) return;
            // Look:
            Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Look(lookInput);
        
            // Bobbing:
            if (movement.IsMoving())
            {
                _timer += Time.deltaTime * bobbingSpeed;
                var localPosition = camHolder.localPosition;
                localPosition = new Vector3(localPosition.x,
                    _midpoint + Mathf.Sin(_timer) * bobbingAmount, localPosition.z);
                camHolder.localPosition = localPosition;
            }
            else
            {
                _timer = 0;
                var localPosition = camHolder.localPosition;
                localPosition = new Vector3(localPosition.x,
                    Mathf.Lerp(localPosition.y, _midpoint, bobbingSpeed * Time.deltaTime), localPosition.z);
                camHolder.localPosition = localPosition;
            }
        }
    }
}
