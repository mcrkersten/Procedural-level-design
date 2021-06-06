using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        private GameManager _gameManager;

        public delegate void StartMousepress(Vector3 mousePosition, int button, KeyCode clickType, InputState state);
        public static event StartMousepress OnStartMousepress;

        public delegate void MousePress(Vector3 mousePosition, int button, KeyCode clickType, InputState state);
        public static event MousePress OnMousePress;

        public delegate void MouseRelease(Vector3 mousePosition, int button, KeyCode clickType, InputState state);
        public static event MouseRelease OnMouseRelease;

        public delegate void ScrollWheelMovement(float movement, KeyCode clickType, InputState state);
        public static event ScrollWheelMovement OnScrollWheelMovement;

        public delegate void NumberButtonpress(int number);
        public static event NumberButtonpress OnNumberButtonpress;

        public delegate void Keypress(int number);
        public static event Keypress OnKeypress;

        private int _lockNumber;
        private bool _clickLock;
        private bool _keyLock;
        private bool _earlyUnlock;
        private bool _reset;
        private bool _hasChangedStateInLock;
        private KeyCode _key;
        private int _hovers = 0;

        public InputState _inputState { private set; get; }
        private InputState _lateChange;
        private InputState _beforeUIHover;
        private InputState _beforePaused;

        public void ChangeInputState(InputState newState)
        {
            if (!_clickLock || newState == InputState.UNIT_SELECTION)
            {
                _inputState = newState;
                _beforeUIHover = newState;
            }
            else
            {
                _hasChangedStateInLock = true;
                _lateChange = newState;
            }
        }

        public void EnterUIHover()
        {
            _hovers++;
            if(_inputState != InputState.UNIT_SELECTION && _inputState != InputState.MENU_HOVER)
            {
                _beforeUIHover = _inputState;
                _inputState = InputState.MENU_HOVER;
            }
        }

        public void ExitUIHover()
        {
            _hovers--;
            if(_hovers <= 0)
            {
                _inputState = _beforeUIHover;
                _hovers = 0;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _gameManager = GameManager._instance;
        }

        private void Update()
        {
            if(_gameManager._gameState != GameState.PAUSED && _gameManager._gameState != GameState.CUTSCENE)
            {
                MouseButton(0);
                MouseButton(1);
                MouseButton(2);
                ScrollWheel();
                NumberButton();
                CombinationKey();
                KeyButton();
            }
        }

        private void MouseButton(int button)
        {
            SetLock(button);
            ReleaseLock(button);
            OnLateChange(button);
            MouseButtonClick(button, _inputState);
        }

        private void OnLateChange(int button)
        {
            if (Input.GetMouseButtonUp(button) && _lockNumber == button && _hasChangedStateInLock)
            {
                _hasChangedStateInLock = false;
                ChangeInputState(_lateChange);
            }
        }

        private void NumberButton()
        {
            switch (_inputState)
            {
                case InputState.UNIT_MOVEMENT:
                    NumberButtonOnUnitMovement();
                    break;
                case InputState.BUILDING_PLACEMENT:
                    break;
                case InputState.MENU_HOVER:
                    break;
                default:
                    break;
            }
        }

        private void KeyButton()
        {
            switch (_inputState)
            {
                case InputState.UNIT_MOVEMENT:
                    KeyButtonOnUnitMovement();
                    break;
                case InputState.BUILDING_PLACEMENT:
                    break;
                case InputState.MENU_HOVER:
                    break;
                default:
                    break;
            }
        }

        private void MouseButtonClick(int button, InputState state)
        {
            if (Input.GetMouseButtonDown(button))
            {
                OnStartMousepress?.Invoke(Input.mousePosition, button, _key, _inputState);
                _keyLock = true;
            }

            if (Input.GetMouseButton(button) && _lockNumber == button)
                OnMousePress?.Invoke(Input.mousePosition, button, _key, _inputState);

            if (Input.GetMouseButtonUp(button) && _lockNumber == button)
            {
                OnMouseRelease?.Invoke(Input.mousePosition, button, _key, _inputState);
                if (_earlyUnlock)
                {
                    _key = KeyCode.None;
                    _earlyUnlock = false;
                }
            }
        }

        private void NumberButtonOnUnitMovement()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    switch (vKey)
                    {
                        case KeyCode.Alpha0:
                            OnNumberButtonpress?.Invoke(0);
                            break;
                        case KeyCode.Alpha1:
                            OnNumberButtonpress?.Invoke(1);
                            break;
                        case KeyCode.Alpha2:
                            OnNumberButtonpress?.Invoke(2);
                            break;
                        case KeyCode.Alpha3:
                            OnNumberButtonpress?.Invoke(3);
                            break;
                        case KeyCode.Alpha4:
                            OnNumberButtonpress?.Invoke(4);
                            break;
                        case KeyCode.Alpha5:
                            OnNumberButtonpress?.Invoke(5);
                            break;
                        case KeyCode.Alpha6:
                            OnNumberButtonpress?.Invoke(6);
                            break;
                        case KeyCode.Alpha7:
                            OnNumberButtonpress?.Invoke(7);
                            break;
                        case KeyCode.Alpha8:
                            OnNumberButtonpress?.Invoke(8);
                            break;
                        case KeyCode.Alpha9:
                            OnNumberButtonpress?.Invoke(9);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void KeyButtonOnUnitMovement()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    switch (vKey)
                    {
                        case KeyCode.Escape:
                            OnKeypress?.Invoke(0);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CombinationKey()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (!_keyLock)
                {
                    if (Input.GetKeyUp(vKey) && vKey == _key)
                        _key = KeyCode.None;

                    if (Input.GetKeyDown(vKey) && _key == KeyCode.None)
                    {
                        switch (vKey)
                        {
                            case KeyCode.LeftShift:
                                _key = KeyCode.LeftShift;
                                return;
                            case KeyCode.RightShift:
                                _key = KeyCode.LeftShift;
                                return;
                            case KeyCode.LeftAlt:
                                _key = KeyCode.LeftAlt;
                                return;
                            case KeyCode.RightAlt:
                                _key = KeyCode.LeftAlt;
                                return;
                            case KeyCode.LeftControl:
                                _key = KeyCode.LeftControl;
                                return;
                            case KeyCode.RightControl:
                                _key = KeyCode.LeftControl;
                                return;
                            default:
                                return;
                        }
                    }
                }
                else if(Input.GetKeyUp(vKey) && vKey == _key && !_earlyUnlock)
                {
                    _earlyUnlock = true;
                }
                else if (Input.GetKeyUp(vKey) && vKey == _key && _earlyUnlock)
                {
                    _earlyUnlock = false;
                }
            }
        }

        private void ScrollWheel()
        {
            if(Input.mouseScrollDelta.y != 0f)
            {
                switch (_inputState)
                {
                    case InputState.UNIT_MOVEMENT:
                        OnScrollWheelMovement?.Invoke(Input.mouseScrollDelta.y, _key, _inputState);
                        break;
                    case InputState.BUILDING_PLACEMENT:
                        OnScrollWheelMovement?.Invoke(Input.mouseScrollDelta.y, _key, _inputState);
                        break;
                    case InputState.MENU_HOVER:
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetLock(int button)
        {
            if (Input.GetMouseButtonDown(button) && !_clickLock)
            {
                _lockNumber = button;
                _clickLock = true;
            }
        }

        private void ReleaseLock(int button)
        {
            if (Input.GetMouseButtonUp(button) && _lockNumber == button)
            {
                _keyLock = false;
                _clickLock = false;
                if (_reset)
                {
                    _reset = false;
                    _key = KeyCode.None;
                }
            }
        }
    }
}

public enum InputState
{
    UNIT_MOVEMENT = 0,
    UNIT_SELECTION,
    BUILDING_PLACEMENT,
    MENU_HOVER
}
