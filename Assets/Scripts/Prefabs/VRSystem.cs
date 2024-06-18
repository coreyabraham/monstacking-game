using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class VRSystem : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] private XRDeviceSimulator deviceSimulator;

    [field: Space(5)]

    [field: Header("Settings")]
    [field: SerializeField] private bool EnableDeviceEmulator { get; set; } = false;

    private void Awake()
    {
        if (!deviceSimulator) return;
        deviceSimulator.gameObject.SetActive(EnableDeviceEmulator);
    }
}
