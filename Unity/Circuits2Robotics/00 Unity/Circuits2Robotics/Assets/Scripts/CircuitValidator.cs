using UnityEngine;

/// <summary>
/// Validates the three required connections in the
/// battery-resistor-LED prototype.
///
/// A completed valid circuit uses LED_On.
/// All other configurations use LED_Off.
/// </summary>
public class CircuitValidator : MonoBehaviour
{
    [Header("Battery")]
    [SerializeField] private ConnectionPoint batteryPositive;
    [SerializeField] private ConnectionPoint batteryNegative;

    [Header("Resistor")]
    [SerializeField] private ConnectionPoint resistorTop;
    [SerializeField] private ConnectionPoint resistorBottom;

    [Header("LED")]
    [SerializeField] private ConnectionPoint ledAnode;
    [SerializeField] private ConnectionPoint ledCathode;

    [Header("LED Materials")]
    [SerializeField] private Renderer ledRenderer;
    [SerializeField] private Material ledOff;
    [SerializeField] private Material ledOn;

    // Stores the previous validation result.
    private bool isCircuitComplete;

    private void Start()
    {
        // Set the correct LED material at startup.
        Validate();
    }

    /// <summary>
    /// Determines whether two specific poles share
    /// the same completed wire.
    /// </summary>
    private bool AreConnected(
        ConnectionPoint first,
        ConnectionPoint second)
    {
        if (first == null || second == null)
            return false;

        Wire wire = first.ConnectedWire;

        if (wire == null || !wire.IsComplete)
            return false;

        // Wires are bidirectional for connection checking.
        bool forward =
            wire.StartPoint == first &&
            wire.EndPoint == second;

        bool reverse =
            wire.StartPoint == second &&
            wire.EndPoint == first;

        return forward || reverse;
    }

    /// <summary>
    /// Validates the basic circuit configuration.
    /// </summary>
    public void Validate()
    {
        // Battery + -> Resistor Top
        // Resistor Bottom -> LED Anode
        bool configurationA =
            AreConnected(batteryPositive, resistorTop) &&
            AreConnected(resistorBottom, ledAnode);

        // Battery + -> Resistor Bottom
        // Resistor Top -> LED Anode
        bool configurationB =
            AreConnected(batteryPositive, resistorBottom) &&
            AreConnected(resistorTop, ledAnode);

        // LED Cathode -> Battery -
        bool negativeConnection =
            AreConnected(ledCathode, batteryNegative);

        bool valid =
            (configurationA || configurationB) &&
            negativeConnection;

        // Log only when the validity changes.
        if (valid != isCircuitComplete)
        {
            isCircuitComplete = valid;

            Debug.Log(
                valid
                    ? "Circuit complete: LED ON"
                    : "Circuit incomplete: LED OFF"
            );
        }

        UpdateLEDMaterial();
    }

    /// <summary>
    /// Switches between the existing LED materials.
    /// </summary>
    private void UpdateLEDMaterial()
    {
        if (ledRenderer == null)
            return;

        Material targetMaterial =
            isCircuitComplete ? ledOn : ledOff;

        if (targetMaterial != null)
        {
            ledRenderer.sharedMaterial = targetMaterial;
        }
    }
}
