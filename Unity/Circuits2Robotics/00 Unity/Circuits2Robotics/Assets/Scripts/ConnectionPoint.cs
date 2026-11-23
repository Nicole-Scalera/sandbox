using UnityEngine;

/// <summary>
/// Represents a single electrical connection point.
/// A pole can have at most one completed wire attached.
/// </summary>
public class ConnectionPoint : MonoBehaviour
{
    // Identifies each of the six circuit poles.
    public enum PoleType
    {
        BatteryPositive,
        BatteryNegative,
        ResistorTop,
        ResistorBottom,
        LEDAnode,
        LEDCathode
    }

    [Header("Pole Configuration")]
    [SerializeField] private PoleType poleType;

    [Header("Optional Attachment Transform")]
    [SerializeField] private Transform connectionAnchor;

    // Expose the pole type without allowing external
    // scripts to change the serialized value directly.
    public PoleType Type => poleType;

    // The currently attached completed wire.
    public Wire ConnectedWire { get; private set; }

    /// <summary>
    /// World position where a wire visually attaches.
    /// Defaults to the pole's Transform position.
    /// </summary>
    public Vector3 ConnectionPosition
    {
        get
        {
            return connectionAnchor != null
                ? connectionAnchor.position
                : transform.position;
        }
    }

    /// <summary>
    /// Stores a newly completed wire.
    /// Called by WireManager.
    /// </summary>
    public void SetWire(Wire wire)
    {
        ConnectedWire = wire;
    }

    /// <summary>
    /// Removes a connection reference only when it
    /// matches the wire being removed.
    /// </summary>
    public void ClearWire(Wire wire)
    {
        if (ConnectedWire == wire)
            ConnectedWire = null;
    }
}