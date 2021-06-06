using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UnitsAndFormation
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController _instance;
        private GameManager _gameManager;
        private Vector3 _positionLastFrame;
        private Vector3 _rotationLastFrame;
        private Vector3 _startPosition;
        private Vector3 _startPosition3D;
        public float _dragSpeed;
        public LayerMask _groundLayer;

        [SerializeField] private Transform _cameraZoomPoint;
        private Camera _cam;
        private Transform _cameraMovementIconTransform;
        private Image _cameraMovementImage;
        [SerializeField] private Sprite _yRotation;
        [SerializeField] private Sprite _xRotation;
        [SerializeField] private Sprite _movement;
        [SerializeField] private GameObject _cameraCursorPrefab;
        private bool _uiHover;
        public Vector4 _completedControlls;

        [SerializeField] private Rigidbody _rigidbody;

        private void Awake()
        {
            _instance = this;
            InputManager.OnStartMousepress += OnMouseStart;
            InputManager.OnMousePress += OnMousePress;
            InputManager.OnMouseRelease += OnMouseRelease;
            InputManager.OnScrollWheelMovement += OnScrollWheelMovement;
            _completedControlls = Vector4.one;

        }

        private void Start()
        {
            _cam = this.GetComponent<Camera>();
            _gameManager = GameManager._instance;
            GameObject parent = GameObject.FindGameObjectWithTag("UI");
            GameObject inst = Instantiate(_cameraCursorPrefab, parent.transform);
            _cameraMovementIconTransform = inst.transform;
            _cameraMovementImage = _cameraMovementIconTransform.gameObject.GetComponent<Image>();
        }

        public void TransformToTarget(Transform target, Ease easeType, float time)
        {
            _cam.transform.DOMove(target.position, time).SetEase(easeType);
            _cam.transform.DORotate(new Vector3(target.eulerAngles.x, target.eulerAngles.y, 0f), time).SetEase(easeType);
        }

        private void FixedUpdate()
        {

            if (_gameManager._gameState != GameState.CUTSCENE && _gameManager._gameState != GameState.PAUSED)
            {
                _cam.transform.position = Vector3.Lerp(_cam.transform.position, _cameraZoomPoint.position, .1f);
                _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, _cameraZoomPoint.rotation, .1f);
            }

            RaycastHit ray;
            if (Physics.Raycast(transform.parent.position + new Vector3(0, 50, 0), Vector3.down, out ray, Mathf.Infinity, _groundLayer))
            {
                Vector2 point = _cam.WorldToScreenPoint(ray.point);
                _cameraMovementIconTransform.GetComponent<RectTransform>().position = point;
            }
        }

        public void UnparentCameraAfterSpawn()
        {
            this.transform.parent.transform.parent.transform.parent = null;
            this.transform.parent.transform.parent.transform.eulerAngles = new Vector3(0, this.transform.parent.transform.parent.eulerAngles.y, 0);
        }

        public void CenterCameraOnObject(GameObject _focusObject)
        {
            this.transform.parent.DOMove(_focusObject.transform.position, 1f);
        }

        private void OnMouseStart(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (_state == InputState.MENU_HOVER)
            {
                _uiHover = true;
                return;
            }
            _uiHover = false;
            if (x == 1)
            {
                _cameraMovementIconTransform.gameObject.SetActive(true);
                switch (type)
                {
                    case KeyCode.None:
                        _startPosition = position;
                        _startPosition3D = HelperFunctions.GetMousePositionIn3D(position, _groundLayer);
                        _cameraMovementImage.sprite = _movement;
                        break;
                    case KeyCode.LeftAlt:
                        _positionLastFrame = position;
                        _cameraMovementImage.sprite = _yRotation;
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case KeyCode.LeftAlt:
                        _cameraMovementIconTransform.gameObject.SetActive(true);
                        _cameraMovementImage.sprite = _xRotation;
                        _rotationLastFrame = position;
                        break;
                }
            }
        }

        private void OnMousePress(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (_state == InputState.MENU_HOVER || _uiHover)
                return;

            if (x == 1)
            {
                switch (type)
                {
                    case KeyCode.None:
                        MoveCamera(position);
                        break;
                    case KeyCode.LeftAlt:
                        RotateCameraYaxis(position);
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case KeyCode.LeftAlt:
                        RotateCameraXaxis(position);
                        break;
                }
            }
        }

        private void OnMouseRelease(Vector3 position, int x, KeyCode type, InputState _state)
        {
            if (x == 1 || x == 0)
            {
                _cameraMovementIconTransform.gameObject.SetActive(false);
            }
        }

        private void OnScrollWheelMovement(float movement, KeyCode type, InputState _state)
        {
            if (_state == InputState.MENU_HOVER)
                return;

            if (type != KeyCode.None) return;
            Vector3 clamp = _cameraZoomPoint.localPosition + new Vector3(0, 0, movement);
            clamp.z = Mathf.Clamp(clamp.z, -50f, -2f);

            _cameraZoomPoint.localPosition = clamp;

            if (_completedControlls[2] == 0)
            {
                TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.CAMERA_CONTROLS, 2);
                TipsAndGuidesManager._instance.AdvanceGuide(Guide.CAMERA_CONTROLS, 3);
                _completedControlls = new Vector4(1, 1, 1, 0);
            }
        }

        private void MoveCamera(Vector3 position)
        {
            Vector3 _offset = _startPosition3D - HelperFunctions.GetMousePositionIn3D(position, _groundLayer);
//            transform.parent.transform.parent.position += new Vector3(_offset.x, 0, _offset.z);

            _rigidbody.velocity += new Vector3(_offset.x, 0, _offset.z);

            //Indicator
            _cameraMovementIconTransform.eulerAngles = new Vector3(90f - transform.parent.eulerAngles.x, 0, 0);

            if (_completedControlls[0] == 0)
            {
                TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.CAMERA_CONTROLS, 0);
                TipsAndGuidesManager._instance.AdvanceGuide(Guide.CAMERA_CONTROLS, 1);
                _completedControlls = new Vector4(1, 0, 1, 1);
            }
        }

        private void RotateCameraYaxis(Vector3 position)
        {
            float d = (_positionLastFrame.x - position.x);
            transform.parent.transform.parent.eulerAngles += new Vector3(0, -d/3f, 0);
            _positionLastFrame = position;

            //Indicator
            _cameraMovementIconTransform.eulerAngles = new Vector3(90f - transform.parent.eulerAngles.x, 0, 0);

            if (_completedControlls[1] == 0)
            {
                TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.CAMERA_CONTROLS, 1);
                TipsAndGuidesManager._instance.AdvanceGuide(Guide.CAMERA_CONTROLS, 2);
                _completedControlls = new Vector4(1, 1, 0, 1);
            }
        }

        private void RotateCameraXaxis(Vector3 position)
        {
            float d = (_rotationLastFrame.y - position.y);

            Vector3 clamp = transform.parent.transform.eulerAngles + new Vector3(d / 3f, 0, 0);
            clamp.x = Mathf.Clamp(clamp.x, 10f, 89f);

            transform.parent.transform.eulerAngles = clamp;
            _rotationLastFrame = position;

            _cameraMovementIconTransform.eulerAngles = new Vector3(0, 0, 0);

            if (_completedControlls[3] == 0)
            {
                TipsAndGuidesManager._instance._objectiveBuilder.OnCompleteObjective(Guide.CAMERA_CONTROLS, 3);
                _completedControlls[3] = 1;
            }
        }

        private void OnDestroy()
        {
            InputManager.OnStartMousepress -= OnMouseStart;
            InputManager.OnMousePress -= OnMousePress;
            InputManager.OnMouseRelease -= OnMouseRelease;
            InputManager.OnScrollWheelMovement -= OnScrollWheelMovement;
        }
    }
}
