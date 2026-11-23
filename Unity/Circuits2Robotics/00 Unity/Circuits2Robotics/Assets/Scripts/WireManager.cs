
using UnityEngine;

/// <summary>
/// Manages creating, previewing, selecting, replacing,
/// and removing rectangular wires.
///
/// Input comes from PlayerControlsInputData.
/// This class does not directly use the Input System.
/// </summary>
public class WireManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerControlsInputData inputData;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Wire wirePrefab;
    [SerializeField] private CircuitValidator circuitValidator;

    [Header("Wire Appearance")]
    [SerializeField, Min(0.001f)]
    private float wireThickness = 0.1f;

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Material selectedMaterial;

    [Header("Raycasting")]
    [SerializeField] private float maxRayDistance = 500f;

    // LayerMask used to detect the poles and wires.
    [SerializeField] private LayerMask interactionLayers = ~0;

    // === Wire Data ===
    // Current unfinished wire.
    private Wire previewWire;

    // Currently selected completed wire.
    private Wire selectedWire;

    // Pole from which the preview originates.
    private ConnectionPoint startingPoint;
    // =================

    // === Input Data ===
    // Was the left mouse button pressed in the last frame?
    private bool leftClickPressed;
    
    // Mouse position in screen coordinates from the last frame.
    private Vector2 mousePosition;
    
    // All colliders under the mouse cursor from the last raycast.
    // private GameObject[] allCollidersUnderMouse;
    private RaycastHit[] allCollidersUnderMouse;
    // ==================

    private void Awake()
    {
        if (sceneCamera == null)
            sceneCamera = Camera.main;
    }

    private void OnEnable()
    {
        // ALBATROSS
        // Subscribe to our centralized input event.
        // if (inputData != null)
        //     inputData.LeftClicked += HandleLeftClick;
    }

    private void OnDisable()
    {
        // ALBATROSS
        // if (inputData != null)
        //     inputData.LeftClicked -= HandleLeftClick;
        
        // Prevent leftover unfinished wires.
        CancelPreview();
    }

    private void Update()
    {
        
        leftClickPressed = inputData.interaction_LeftClick;
        mousePosition = inputData.mousePositionScreen;
        allCollidersUnderMouse = inputData.allRaycastHits;
        
        // ALBATROSS
        // Only the preview needs continuous mouse updates.
        // if (previewWire != null && inputData != null)
        //     UpdatePreview(inputData.MousePosition);

        HandleLeftClick();
        

    }

    /// <summary>
    /// Receives the screen position from the input handler.
    /// Determines what the player clicked.
    /// </summary>
    private void HandleLeftClick()
    {
        // If the Player performed a left click
        if (!leftClickPressed)
            return;
        
        if (sceneCamera == null)
            return;

        Physics.SyncTransforms();

        // Pole selection takes priority over wire selection.
        ConnectionPoint clickedPole = FindClosestPole(allCollidersUnderMouse);

        if (clickedPole != null)
        {
            HandlePoleClick(clickedPole);
            return;
        }

        // Only completed wires are selectable.
        if (previewWire == null)
        {
            Wire clickedWire = FindClosestWire(allCollidersUnderMouse);
            SelectWire(clickedWire);
        }
    }

    /// <summary>
    /// Returns the nearest valid pole in the raycast results.
    /// </summary>
    private ConnectionPoint FindClosestPole(RaycastHit[] hits)
    {
        ConnectionPoint closest = null;
        float nearestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            ConnectionPoint point = hit.collider.GetComponentInParent<ConnectionPoint>();

            if (point == null)
                continue;

            if (hit.distance < nearestDistance)
            {
                closest = point;
                nearestDistance = hit.distance;
            }
        }

        return closest;
    }

    /// <summary>
    /// Returns the nearest completed wire in the results.
    /// </summary>
    private Wire FindClosestWire(RaycastHit[] hits)
    {
        Wire closest = null;
        float nearestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            Wire wire =
                hit.collider.GetComponentInParent<Wire>();

            if (wire == null || !wire.IsComplete)
                continue;

            if (hit.distance < nearestDistance)
            {
                closest = wire;
                nearestDistance = hit.distance;
            }
        }

        return closest;
    }

    /// <summary>
    /// Starts a preview or completes an existing preview.
    /// </summary>
    private void HandlePoleClick(ConnectionPoint point)
    {
        if (previewWire == null)
        {
            BeginPreview(point);
            return;
        }

        // Do not connect a pole to itself.
        if (point == startingPoint)
            return;

        FinishConnection(point);
    }

    /// <summary>
    /// Instantiates an unfinished wire at the first pole.
    /// </summary>
    private void BeginPreview(ConnectionPoint point)
    {
        if (wirePrefab == null)
        {
            Debug.LogError("WireManager: Missing Wire prefab.");
            return;
        }

        // Remove any selection highlight.
        SelectWire(null);

        startingPoint = point;

        // No parent: wire is independent of circuit objects.
        previewWire = Instantiate(wirePrefab);
        previewWire.name = "Wire_Preview";

        previewWire.InitializePreview(
            startingPoint,
            wireThickness
        );

        SetWireMaterial(previewWire, previewMaterial);

        // Preview wires must not interfere with raycasts.
        Collider collider = previewWire.GetComponent<Collider>();

        if (collider != null)
            collider.enabled = false;

        // ALBATROSS
        // if (inputData != null)
        //     UpdatePreview(inputData.MousePosition);
        
        UpdatePreview(mousePosition);
    }

    /// <summary>
    /// Makes the preview follow the cursor.
    /// Snaps its free end to a valid pole when targeted.
    /// </summary>
    private void UpdatePreview(Vector2 mousePos)
    {
        // Ensure all required references are present.
        if (previewWire == null ||
            startingPoint == null ||
            sceneCamera == null)
            return;

        Physics.SyncTransforms();

        // ALBATROSS
        Ray ray = sceneCamera.ScreenPointToRay(
            mousePosition
        );
        
        ConnectionPoint target = FindClosestPole(allCollidersUnderMouse);

        if (target != null && target != startingPoint)
        {
            // Snap directly to the target pole.
            previewWire.SetPreviewPosition(
                target.ConnectionPosition
            );

            return;
        }

        // Create an invisible plane through the
        // starting pole, perpendicular to the camera.
        Plane plane = new Plane(
            sceneCamera.transform.forward,
            startingPoint.ConnectionPosition
        );

        if (plane.Raycast(ray, out float distance))
        {
            previewWire.SetPreviewPosition(
                ray.GetPoint(distance)
            );
        }
    }

    /// <summary>
    /// Completes the current wire, replacing any conflicting
    /// connections on either endpoint.
    /// </summary>
    private void FinishConnection(ConnectionPoint endPoint)
    {
        if (previewWire == null || startingPoint == null)
            return;

        Wire newWire = previewWire;

        // Free the first pole if it is occupied.
        if (startingPoint.ConnectedWire != null)
            RemoveWire(startingPoint.ConnectedWire);

        // Free the second pole if it is occupied.
        if (endPoint.ConnectedWire != null)
            RemoveWire(endPoint.ConnectedWire);

        // Make this a completed connection.
        newWire.Complete(endPoint);

        // Register both endpoint references.
        startingPoint.SetWire(newWire);
        endPoint.SetWire(newWire);

        newWire.name =
            $"Wire_{startingPoint.Type}_To_{endPoint.Type}";

        // Apply the completed wire appearance.
        SetWireMaterial(newWire, normalMaterial);

        // Allow clicking the finished wire.
        Collider collider = newWire.GetComponent<Collider>();

        if (collider != null)
            collider.enabled = true;

        // Clear the temporary creation state.
        previewWire = null;
        startingPoint = null;

        ValidateCircuit();
    }

    /// <summary>
    /// Cancels an unfinished wire without removing
    /// any completed connections.
    ///
    /// Can be called by a future Cancel input event.
    /// </summary>
    public void CancelPreview()
    {
        if (previewWire == null)
            return;

        Destroy(previewWire.gameObject);

        previewWire = null;
        startingPoint = null;
    }

    /// <summary>
    /// Selects a wire and applies a highlight.
    /// Clicking empty space deselects it.
    /// </summary>
    private void SelectWire(Wire wire)
    {
        if (selectedWire != null)
            SetWireMaterial(selectedWire, normalMaterial);

        selectedWire = wire;

        if (selectedWire != null)
            SetWireMaterial(selectedWire, selectedMaterial);
    }

    /// <summary>
    /// Public method for deleting the selected wire.
    ///
    /// Ready to be connected to a Delete input event.
    /// </summary>
    public void DeleteSelectedWire()
    {
        if (previewWire != null || selectedWire == null)
            return;

        RemoveWire(selectedWire);
    }

    /// <summary>
    /// Destroys a completed wire and releases its poles.
    /// </summary>
    private void RemoveWire(Wire wire)
    {
        if (wire == null)
            return;

        // Release both endpoint references.
        if (wire.StartPoint != null)
            wire.StartPoint.ClearWire(wire);

        if (wire.EndPoint != null)
            wire.EndPoint.ClearWire(wire);

        if (selectedWire == wire)
            selectedWire = null;

        // Disable its collider immediately so it cannot
        // be selected again before Destroy takes effect.
        Collider collider = wire.GetComponent<Collider>();

        if (collider != null)
            collider.enabled = false;

        Destroy(wire.gameObject);

        ValidateCircuit();
    }

    /// <summary>
    /// Applies a visual material if one was assigned.
    /// </summary>
    private void SetWireMaterial(Wire wire, Material material)
    {
        if (wire == null || material == null)
            return;

        Renderer renderer = wire.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = material;
    }

    /// <summary>
    /// Asks the validator to check the circuit.
    /// </summary>
    private void ValidateCircuit()
    {
        if (circuitValidator != null)
            circuitValidator.Validate();
    }
}
