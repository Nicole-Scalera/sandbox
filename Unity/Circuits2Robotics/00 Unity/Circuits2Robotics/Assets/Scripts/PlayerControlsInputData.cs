using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInputData : MonoBehaviour
{
    private PlayerControls input;
    
    #region Input Handling Variables
    
    public bool interaction_LeftClick;
    public bool interaction_RightClick;
    public bool interaction_Pause;

    #endregion
	
    private void Awake()
    {
        // Create a new instance of the PlayerControls class
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        SubscribeAllActions();
    }
	
    private void OnDisable()
    {
        UnsubscribeAllActions();
    }

    #region OnEnable, OnDisable, OnDestroy

        private void SubscribeAllActions()
	{
		// Left Click
		input.Interaction.LeftClick.started += Interaction_Interact;
		input.Interaction.LeftClick.performed += Interaction_Interact;
		input.Interaction.LeftClick.canceled += Interaction_Interact;

		input.Interaction.Enable();
	}

	private void UnsubscribeAllActions()
	{
        // Left Click
		input.Interaction.LeftClick.started -= Interaction_Interact;
		input.Interaction.LeftClick.performed -= Interaction_Interact;
		input.Interaction.LeftClick.canceled -= Interaction_Interact;
        
        input.Interaction.Disable();
	}
    
    private void OnDestroy()
    {
        // Release the Input Actions instance.
        input?.Dispose();
    }

    #endregion
    
    
    private void Interaction_Interact(InputAction.CallbackContext context)
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