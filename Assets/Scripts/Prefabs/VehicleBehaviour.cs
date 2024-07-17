using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleBehaviour : MonoBehaviour
{
    [HideInInspector] public Vector3 vehicleDestination;
    [HideInInspector] public float vehicleSpeed = 1.0f;

    [HideInInspector] public Audible vehicleVoice;
    [HideInInspector] public VehicleHandler vehicleHandler;

    [HideInInspector] public bool insideVolume = false;

    private XRGrabInteractable xrInteractable;

    private bool allowMovement = true;

    private bool deletionSet = false;
    private bool voicePlayed = false;

    private float currentTimeout = 0.0f;
    private float maxTimeout = 3.0f;

    public void OnVehicleGrab(SelectEnterEventArgs eventArgs)
    {
        allowMovement = false;
        ResetTimer(false);

        if (!voicePlayed)
        {
            voicePlayed = true;
            if (vehicleVoice != null) AudioHandler.Instance.PlayOnce(vehicleVoice);
        }
    }

    public void OnVehicleLetGo(SelectExitEventArgs eventArgs) => ResetTimer(true);

    public void ResetTimer(bool deletionStatus)
    {
        deletionSet = deletionStatus;
        currentTimeout = 0.0f; 
    }

    private void FixedUpdate()
    {
        if (!allowMovement) return;

        Vector3 moveToPosition = Vector3.MoveTowards(gameObject.transform.position, vehicleDestination, Time.fixedDeltaTime * vehicleSpeed);
        gameObject.transform.position = moveToPosition;

        if (gameObject.transform.position != vehicleDestination) return;
        vehicleHandler.DestroyVehicle(gameObject);
    }

    private void Update()
    {
        if (!deletionSet) return;

        if (currentTimeout >= maxTimeout && !insideVolume)
        {
            xrInteractable.enabled = false;
            vehicleHandler.DestroyVehicle(gameObject);
            return;
        }

        currentTimeout += Time.deltaTime;
    }

    private void Awake() => xrInteractable = gameObject.GetComponent<XRGrabInteractable>();
}
