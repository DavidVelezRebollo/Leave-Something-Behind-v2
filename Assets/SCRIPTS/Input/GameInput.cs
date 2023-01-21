//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/SCRIPTS/Input/GameInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""InputCharacter"",
            ""id"": ""4e298424-3be6-4872-8932-bd6a933b2f0d"",
            ""actions"": [
                {
                    ""name"": ""InputMovement"",
                    ""type"": ""Value"",
                    ""id"": ""50f60697-1b5d-498f-9f6a-a13aae47c059"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputShootPosition"",
                    ""type"": ""Value"",
                    ""id"": ""be24d9cf-ebfc-4da3-8d8d-f487cc2dea15"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""f7b3d687-5b78-49d5-84ea-ff597bffe6ea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b9b8cae3-4c17-4e1e-9d70-bf1b29bdfb4f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""22dfe34c-1062-4d11-a6cf-c339ccff1454"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42dad1d5-0363-48a2-99f5-17b7a1381150"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5009b2ed-99da-43b6-a5e8-486a1641c1f0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""3cd2872f-368c-45bf-80ff-1c5a4b06958b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ce36922d-de82-4e30-a43b-f04374ad4f07"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b90bff0f-2343-4b08-bfbf-659a5b783b28"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ad81779-9f3f-4dc6-a486-cbd34b58cd26"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""059f8e3f-67e4-4452-bed8-14f90e3fdf4c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""35518d89-9f17-48eb-a6f1-ba419f8682ba"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""InputShootPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7516d7b7-673b-4994-8044-728d83c5d293"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InputShootPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Buttons"",
            ""id"": ""25efdd87-30c9-472c-a51e-582209414000"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""d0b2d651-5175-442b-bb71-4e0fcc355a5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6da821ea-cec9-447f-a9b3-05ccd951d7e5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""529704ef-3692-4ca1-8ba8-77d805bcd018"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // InputCharacter
        m_InputCharacter = asset.FindActionMap("InputCharacter", throwIfNotFound: true);
        m_InputCharacter_InputMovement = m_InputCharacter.FindAction("InputMovement", throwIfNotFound: true);
        m_InputCharacter_InputShootPosition = m_InputCharacter.FindAction("InputShootPosition", throwIfNotFound: true);
        // Buttons
        m_Buttons = asset.FindActionMap("Buttons", throwIfNotFound: true);
        m_Buttons_Pause = m_Buttons.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // InputCharacter
    private readonly InputActionMap m_InputCharacter;
    private IInputCharacterActions m_InputCharacterActionsCallbackInterface;
    private readonly InputAction m_InputCharacter_InputMovement;
    private readonly InputAction m_InputCharacter_InputShootPosition;
    public struct InputCharacterActions
    {
        private @GameInput m_Wrapper;
        public InputCharacterActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @InputMovement => m_Wrapper.m_InputCharacter_InputMovement;
        public InputAction @InputShootPosition => m_Wrapper.m_InputCharacter_InputShootPosition;
        public InputActionMap Get() { return m_Wrapper.m_InputCharacter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputCharacterActions set) { return set.Get(); }
        public void SetCallbacks(IInputCharacterActions instance)
        {
            if (m_Wrapper.m_InputCharacterActionsCallbackInterface != null)
            {
                @InputMovement.started -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputMovement;
                @InputMovement.performed -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputMovement;
                @InputMovement.canceled -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputMovement;
                @InputShootPosition.started -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputShootPosition;
                @InputShootPosition.performed -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputShootPosition;
                @InputShootPosition.canceled -= m_Wrapper.m_InputCharacterActionsCallbackInterface.OnInputShootPosition;
            }
            m_Wrapper.m_InputCharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @InputMovement.started += instance.OnInputMovement;
                @InputMovement.performed += instance.OnInputMovement;
                @InputMovement.canceled += instance.OnInputMovement;
                @InputShootPosition.started += instance.OnInputShootPosition;
                @InputShootPosition.performed += instance.OnInputShootPosition;
                @InputShootPosition.canceled += instance.OnInputShootPosition;
            }
        }
    }
    public InputCharacterActions @InputCharacter => new InputCharacterActions(this);

    // Buttons
    private readonly InputActionMap m_Buttons;
    private IButtonsActions m_ButtonsActionsCallbackInterface;
    private readonly InputAction m_Buttons_Pause;
    public struct ButtonsActions
    {
        private @GameInput m_Wrapper;
        public ButtonsActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Buttons_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Buttons; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ButtonsActions set) { return set.Get(); }
        public void SetCallbacks(IButtonsActions instance)
        {
            if (m_Wrapper.m_ButtonsActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_ButtonsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_ButtonsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_ButtonsActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_ButtonsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public ButtonsActions @Buttons => new ButtonsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IInputCharacterActions
    {
        void OnInputMovement(InputAction.CallbackContext context);
        void OnInputShootPosition(InputAction.CallbackContext context);
    }
    public interface IButtonsActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
