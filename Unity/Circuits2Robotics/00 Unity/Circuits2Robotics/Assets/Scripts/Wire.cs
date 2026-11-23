
using UnityEngine;

/// <summary>
/// Represents a straight rectangular wire between two poles.
/// Supports unfinished previews and completed connections.
/// 
/// Assumes the wire is a unit cube aligned along local Y,
/// with no scaled parent Transform.
/// </summary>
public class Wire : MonoBehaviour
{
    public ConnectionPoint StartPoint { get; private set; }
    public ConnectionPoint EndPoint { get; private set; }

    public bool IsComplete =>
        StartPoint != null && EndPoint != null;

    private float thickness = 0.1f;

    // Free endpoint while the wire is being previewed.
    private Vector3 previewPosition;

    /// <summary>
    /// Assigns the starting pole of an unfinished wire.
    /// </summary>
    public void InitializePreview(
        ConnectionPoint start,
        float wireThickness)
    {
        StartPoint = start;
        EndPoint = null;

        thickness = wireThickness;
        previewPosition = start.ConnectionPosition;

        RefreshVisual();
    }

    /// <summary>
    /// Changes the free endpoint of an unfinished wire.
    /// </summary>
    public void SetPreviewPosition(Vector3 position)
    {
        if (IsComplete)
            return;

        previewPosition = position;
    }

    /// <summary>
    /// Assigns the destination pole.
    /// </summary>
    public void Complete(ConnectionPoint end)
    {
        EndPoint = end;
        RefreshVisual();
    }

    private void LateUpdate()
    {
        // Update after ordinary movement scripts.
        // This keeps the wire attached as components move.
        RefreshVisual();
    }

    /// <summary>
    /// Calculates the wire's center, direction, and length.
    /// </summary>
    private void RefreshVisual()
    {
        if (StartPoint == null)
            return;

        Vector3 start = StartPoint.ConnectionPosition;

        Vector3 end = IsComplete
            ? EndPoint.ConnectionPosition
            : previewPosition;

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // Place the cube halfway between the endpoints.
        transform.position = (start + end) * 0.5f;

        // Rotate the cube's Y-axis toward its endpoint.
        if (distance > 0.0001f)
        {
            transform.rotation = Quaternion.FromToRotation(
                Vector3.up,
                direction / distance
            );
        }

        // Stretch the rectangular cube along local Y.
        transform.localScale = new Vector3(
            thickness,
            distance,
            thickness
        );
    }
}
