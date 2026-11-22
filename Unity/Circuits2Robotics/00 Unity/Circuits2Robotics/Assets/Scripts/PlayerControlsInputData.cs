using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInputData : MonoBehaviour
{
    private PlayerControls input;

    #region Input Handling Variables

    [Header("Interaction")]

    // True while the left mouse button is held.
    public bool interaction_LeftClick;

    // Reserved for future right-click input.
    public bool interaction_RightClick;

    // Reserved for future pause input.
    public bool interaction_Pause;

    #endregion

    private void Awake()
    {
        // Create an instance of the generated wrapper.
        input = new PlayerControls();
    }

    private void Update()
    {
        // Debug.Log("PlayerControlsInputData.cs >> interaction_LeftClick: " + interaction_LeftClick);
        // Debug.Log("PlayerControlsInputData.cs >> interaction_Pause: " + interaction_Pause);
        // Debug.Log("PlayerControlsInputData.cs >> interaction_MousePosition: " + interaction_MousePosition);
    }

    private void FixedUpdate()
    {
        // Create an array of RaycastHit objects to store the results of the raycast.
        // This RaycastAll() method will return all colliders that are hit by the ray, not just the first one.
        // However, I really only need the first one for right now.
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        
        if (hits.Length > 0)
        {
            Debug.Log(hits[0].collider.gameObject.name);
        }
    }

    // Called when the script is enabled, subscribes to input events.
    private void OnEnable()
    {
        SubscribeAllActions();
    }
    
    // Called when the script is disabled, unsubscribes to input events.
    private void OnDisable()
    {
        UnsubscribeAllActions();

        // // Clear the stored button state.
        // interaction_LeftClick = false;
        // leftClickPressedFrame = -1;
    }

    private void OnDestroy()
    {
        // Dispose of the Input Actions instance.
        input?.Dispose();
    }

    #region Input Subscriptions

    // Subscribe to all Input Action events to handle player input.
    private void SubscribeAllActions()
    {
        // Note: Subscribe to the performed, subscribed, or canceled events of the Input Actions you want to handle.
        // For example:
        //
        // input.Interaction.LeftClick.started += Interaction_LeftClick;
        // input.Interaction.LeftClick.performed += Interaction_LeftClick;
        // input.Interaction.LeftClick.canceled += Interaction_LeftClick;
        
        // input = reference to the generated PlayerControls wrapper.
        // Interaction = the action map defined in the Input Actions asset.
        // LeftClick = the action defined in the Interaction action map.
        // performed = the event that is triggered when the action is performed.
        
        input.Interaction.LeftClick.started += Interaction_LeftClick;
        input.Interaction.LeftClick.performed += Interaction_LeftClick;
        input.Interaction.LeftClick.canceled += Interaction_LeftClick;
        
        // Enable the Interaction action map.
        input.Interaction.Enable();
    }

    // Unsubscribe from all Input Action events to prevent memory leaks.
    private void UnsubscribeAllActions()
    {
        input.Interaction.LeftClick.started -= Interaction_LeftClick;
        input.Interaction.LeftClick.performed -= Interaction_LeftClick;
        input.Interaction.LeftClick.canceled -= Interaction_LeftClick;
        
        // Disable the map
        input.Interaction.Disable();
    }

    #endregion
    
    /// <summary>
    /// Reads the current state of the left-click action.
    /// Detects a new press without generating an event.
    /// </summary>
    private void Interaction_LeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interaction_LeftClick = true;
        }
        else if (context.canceled)
        {
            interaction_LeftClick = false;
        }
    }
    
    private void ClickDetected()
    {
        // RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        
        // Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        // {
        //     
        //     SelectableTile tile = hit.collider.GetComponent<SelectableTile>();
        //     if (tile != null)
        //     {
        //         if (selectedTile != null)
        //             selectedTile.Deselect();
        //     
        //
        //         selectedTile = tile;
        //         selectedTile.Select();Physics.RaycastAll
        //     }
        // }
    }
    
    // private void Interaction_LeftClick(InputAction.CallbackContext context)
    // {
    //     bool isPressed = context.ReadValueAsButton();
    //
    //     // Only a transition from released to pressed
    //     // should count as a new click.
    //     if (isPressed && !interaction_LeftClick)
    //     {
    //         leftClickPressedFrame = Time.frameCount;
    //     }
    //
    //     // Store the current button state.
    //     interaction_LeftClick = isPressed;
    // }
    
    //
    //
    // /// <summary>
    // /// Receives a performed LeftClick action.
    // /// Notifies interested gameplay systems.
    // /// </summary>
    // private void Circuit_LeftClick(InputAction.CallbackContext context)
    // {
    //     if (!context.performed ||
    //         !context.ReadValueAsButton())
    //         return;
    //
    //     LeftClicked?.Invoke(MousePosition);
    // }
    //
    // // Broadcasts a single left-click with its screen position.
    // public event Action<Vector2> LeftClicked;
    //
    // /// <summary>
    // /// Current mouse position in screen coordinates.
    // /// Other systems may read this without using the
    // /// Input System API directly.
    // /// </summary>
    // public Vector2 MousePosition
    // {
    //     get
    //     {
    //         if (Mouse.current == null)
    //             return Vector2.zero;
    //
    //         return Mouse.current.position.ReadValue();
    //     }
    // }
}