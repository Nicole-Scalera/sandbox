using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInputData : MonoBehaviour
{
    // Reference to the PlayerControls C# wrapper
    private PlayerControls input;

    #region Input Handling Variables

    [Header("Interaction")]

    // True while the left mouse button is held.
    public bool interaction_LeftClick;

    // Reserved for future right-click input.
    public bool interaction_RightClick;

    // Reserved for future pause input.
    public bool interaction_Pause;
    
    // Layer mask and raycasting information
    public string layerMaskName;
    public GameObject hitObject;

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
        Raycasting_LayerMaskInfo();
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
    
    private void Raycasting_LayerMaskInfo()
    {
        // Create an array of RaycastHit objects to store the results of the raycast.
        // This RaycastAll() method will return all colliders that are hit by the ray, not just the first one.
        // However, I really only need the first one for right now.
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        
        if (hits.Length > 0)
        {
            // Debugging
            var firstHit = hits[0];
            var maskNumber = firstHit.collider.gameObject.layer;
            layerMaskName = LayerMask.LayerToName(maskNumber);
            Debug.Log("PlayerControlsInputData.cs >> First hit: " + layerMaskName);
            
            // Object hit
            hitObject = firstHit.collider.gameObject;
            Debug.Log("PlayerControlsInputData.cs >> Hit object: " + hitObject.name);
            
        }
        else
        {
            layerMaskName = "";
        }
    }
}