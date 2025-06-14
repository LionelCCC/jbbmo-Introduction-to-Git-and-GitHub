using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction attackQAction;
    private InputAction attackEAction;
    private InputAction attackRAction;
    
    // Events
    public event Action<Vector2> OnMoveInput;
    public event Action<Vector2> OnLookInput;
    public event Action<string> OnAttackInput;
    
    void Awake()
    {
        // Get input actions
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        attackQAction = inputActions.FindAction("AttackQ");
        attackEAction = inputActions.FindAction("AttackE");
        attackRAction = inputActions.FindAction("AttackR");
    }
    
    void OnEnable()
    {
        // Enable input actions
        moveAction?.Enable();
        lookAction?.Enable();
        attackQAction?.Enable();
        attackEAction?.Enable();
        attackRAction?.Enable();
        
        // Subscribe to input events
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        
        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;
        
        attackQAction.performed += ctx => OnAttackInput?.Invoke("Q");
        attackEAction.performed += ctx => OnAttackInput?.Invoke("E");
        attackRAction.performed += ctx => OnAttackInput?.Invoke("R");
    }
    
    void OnDisable()
    {
        // Unsubscribe from input events
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        
        lookAction.performed -= OnLook;
        lookAction.canceled -= OnLook;
        
        // Disable input actions
        moveAction?.Disable();
        lookAction?.Disable();
        attackQAction?.Disable();
        attackEAction?.Disable();
        attackRAction?.Disable();
    }
    
    void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);
    }
    
    void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        OnLookInput?.Invoke(lookInput);
    }
}

