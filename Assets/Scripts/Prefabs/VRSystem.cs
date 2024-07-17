using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class VRSystem : MonoBehaviour
{
    [field: Header("Assets")]
    [field: SerializeField] private XRDeviceSimulator deviceSimulator;
    [field: SerializeField] private SkinnedMeshRenderer LeftHand;
    [field: SerializeField] private SkinnedMeshRenderer RightHand;

    [field: Space(5)]

    [field: Header("Settings")]
    [field: SerializeField] private bool EnableDeviceEmulator { get; set; } = false;

    public void LeftHandInput(InputAction.CallbackContext context)
    {
        LeftHand.SetBlendShapeWeight(0, context.ReadValue<float>() * 100);
    }

    public void RightHandInput(InputAction.CallbackContext context)
    {
        RightHand.SetBlendShapeWeight(0, context.ReadValue<float>() * 100);
    }

    private void Awake()
    {
        deviceSimulator.gameObject.SetActive(EnableDeviceEmulator);
    }
}
