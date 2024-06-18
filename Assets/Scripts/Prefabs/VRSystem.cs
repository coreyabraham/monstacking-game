using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class VRSystem : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] private XRDeviceSimulator deviceSimulator;
    [field: SerializeField] private TeleportationProvider teleportProvider;

    [field: Space(5)]

    [field: Header("Settings")]
    [field: SerializeField] private bool EnableDeviceEmulator { get; set; } = false;
    [field: SerializeField] private bool TeleportationToggleable { get; set; } = false;

    private void Awake()
    {
        if (!TeleportationToggleable) teleportProvider.enabled = false;
        deviceSimulator.gameObject.SetActive(EnableDeviceEmulator);
    }

    public void A_Press(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        teleportProvider.enabled = !teleportProvider;
        AudioHandler.Instance.Play("TeleportToggle");
    }
}
