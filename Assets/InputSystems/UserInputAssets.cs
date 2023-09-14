//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystems/UserInputAssets.inputactions
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

public partial class @UserInputAssets: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UserInputAssets()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserInputAssets"",
    ""maps"": [
        {
            ""name"": ""Locomotion"",
            ""id"": ""e205a30d-eba7-44fb-ba2c-d08459025e15"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0be34fab-b269-4797-881e-56c56fdad3ab"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a93679e8-42f5-4dd7-a173-64198c12826a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""View"",
                    ""type"": ""Value"",
                    ""id"": ""e5b80988-8c8f-4685-bf32-95647b687f9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""67393424-4cc7-452c-af8f-2790bb6ecaf9"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""f65e0fce-ef40-4d50-b892-3ae57d7874f7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""56904430-254f-422c-af76-527aebb9c143"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""67d0a29b-1f02-4f5e-b586-54c9ee140c29"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b91d928e-ca81-4e67-9440-75094aa90db7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3d3ec65f-89c0-489c-99c6-21423692648c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1d0c6b69-2094-41fd-86d5-8bff57f410dd"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d6aaa49-a047-441e-8216-857cbb4865b4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3953a060-4b10-4efc-a3a8-11a0fa307f58"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interaction"",
            ""id"": ""40585ff5-0a21-41f2-8e4b-fb80601b6ee1"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""702a53ec-f5fb-4a72-ab7a-406839c86007"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6e694d8d-141f-438c-9c13-65b9b797a34a"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d91da170-d839-4c39-aa1f-93dc7fa0f920"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TouchFallback"",
            ""id"": ""1e310dd9-778b-4b84-afec-19dc9645bfe6"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""3723505c-38d9-461a-8a43-258e453d7257"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftButton"",
                    ""type"": ""Button"",
                    ""id"": ""630e7030-02f2-45a7-bbcd-04b055aa7ca8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""30dd69fe-175b-45f3-acdf-12c5de5274c7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9cd46b5a-0c2f-45bb-bf31-6199058e8e1d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Locomotion
        m_Locomotion = asset.FindActionMap("Locomotion", throwIfNotFound: true);
        m_Locomotion_Move = m_Locomotion.FindAction("Move", throwIfNotFound: true);
        m_Locomotion_Jump = m_Locomotion.FindAction("Jump", throwIfNotFound: true);
        m_Locomotion_View = m_Locomotion.FindAction("View", throwIfNotFound: true);
        // Interaction
        m_Interaction = asset.FindActionMap("Interaction", throwIfNotFound: true);
        m_Interaction_Fire = m_Interaction.FindAction("Fire", throwIfNotFound: true);
        // TouchFallback
        m_TouchFallback = asset.FindActionMap("TouchFallback", throwIfNotFound: true);
        m_TouchFallback_Position = m_TouchFallback.FindAction("Position", throwIfNotFound: true);
        m_TouchFallback_LeftButton = m_TouchFallback.FindAction("LeftButton", throwIfNotFound: true);
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

    // Locomotion
    private readonly InputActionMap m_Locomotion;
    private List<ILocomotionActions> m_LocomotionActionsCallbackInterfaces = new List<ILocomotionActions>();
    private readonly InputAction m_Locomotion_Move;
    private readonly InputAction m_Locomotion_Jump;
    private readonly InputAction m_Locomotion_View;
    public struct LocomotionActions
    {
        private @UserInputAssets m_Wrapper;
        public LocomotionActions(@UserInputAssets wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Locomotion_Move;
        public InputAction @Jump => m_Wrapper.m_Locomotion_Jump;
        public InputAction @View => m_Wrapper.m_Locomotion_View;
        public InputActionMap Get() { return m_Wrapper.m_Locomotion; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LocomotionActions set) { return set.Get(); }
        public void AddCallbacks(ILocomotionActions instance)
        {
            if (instance == null || m_Wrapper.m_LocomotionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LocomotionActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @View.started += instance.OnView;
            @View.performed += instance.OnView;
            @View.canceled += instance.OnView;
        }

        private void UnregisterCallbacks(ILocomotionActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @View.started -= instance.OnView;
            @View.performed -= instance.OnView;
            @View.canceled -= instance.OnView;
        }

        public void RemoveCallbacks(ILocomotionActions instance)
        {
            if (m_Wrapper.m_LocomotionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILocomotionActions instance)
        {
            foreach (var item in m_Wrapper.m_LocomotionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LocomotionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LocomotionActions @Locomotion => new LocomotionActions(this);

    // Interaction
    private readonly InputActionMap m_Interaction;
    private List<IInteractionActions> m_InteractionActionsCallbackInterfaces = new List<IInteractionActions>();
    private readonly InputAction m_Interaction_Fire;
    public struct InteractionActions
    {
        private @UserInputAssets m_Wrapper;
        public InteractionActions(@UserInputAssets wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Interaction_Fire;
        public InputActionMap Get() { return m_Wrapper.m_Interaction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractionActions set) { return set.Get(); }
        public void AddCallbacks(IInteractionActions instance)
        {
            if (instance == null || m_Wrapper.m_InteractionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InteractionActionsCallbackInterfaces.Add(instance);
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
        }

        private void UnregisterCallbacks(IInteractionActions instance)
        {
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
        }

        public void RemoveCallbacks(IInteractionActions instance)
        {
            if (m_Wrapper.m_InteractionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInteractionActions instance)
        {
            foreach (var item in m_Wrapper.m_InteractionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InteractionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InteractionActions @Interaction => new InteractionActions(this);

    // TouchFallback
    private readonly InputActionMap m_TouchFallback;
    private List<ITouchFallbackActions> m_TouchFallbackActionsCallbackInterfaces = new List<ITouchFallbackActions>();
    private readonly InputAction m_TouchFallback_Position;
    private readonly InputAction m_TouchFallback_LeftButton;
    public struct TouchFallbackActions
    {
        private @UserInputAssets m_Wrapper;
        public TouchFallbackActions(@UserInputAssets wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_TouchFallback_Position;
        public InputAction @LeftButton => m_Wrapper.m_TouchFallback_LeftButton;
        public InputActionMap Get() { return m_Wrapper.m_TouchFallback; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchFallbackActions set) { return set.Get(); }
        public void AddCallbacks(ITouchFallbackActions instance)
        {
            if (instance == null || m_Wrapper.m_TouchFallbackActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TouchFallbackActionsCallbackInterfaces.Add(instance);
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
            @LeftButton.started += instance.OnLeftButton;
            @LeftButton.performed += instance.OnLeftButton;
            @LeftButton.canceled += instance.OnLeftButton;
        }

        private void UnregisterCallbacks(ITouchFallbackActions instance)
        {
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
            @LeftButton.started -= instance.OnLeftButton;
            @LeftButton.performed -= instance.OnLeftButton;
            @LeftButton.canceled -= instance.OnLeftButton;
        }

        public void RemoveCallbacks(ITouchFallbackActions instance)
        {
            if (m_Wrapper.m_TouchFallbackActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITouchFallbackActions instance)
        {
            foreach (var item in m_Wrapper.m_TouchFallbackActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TouchFallbackActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TouchFallbackActions @TouchFallback => new TouchFallbackActions(this);
    public interface ILocomotionActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
    }
    public interface IInteractionActions
    {
        void OnFire(InputAction.CallbackContext context);
    }
    public interface ITouchFallbackActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnLeftButton(InputAction.CallbackContext context);
    }
}
