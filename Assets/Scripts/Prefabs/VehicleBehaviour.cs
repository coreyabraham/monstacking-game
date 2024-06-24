using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    public VehicleType vehicleType;
    [HideInInspector] public Vector3 vehicleDestination;

    private float vehicleSpeed = 0.0f;

    private void FixedUpdate()
    {
        Vector3 moveToPosition = Vector3.MoveTowards(gameObject.transform.position, vehicleDestination, Time.fixedDeltaTime * vehicleSpeed);
        gameObject.transform.position = moveToPosition;

        if (gameObject.transform.position != vehicleDestination) return;
        Destroy(gameObject);
    }

    private void Awake()
    {
        switch (vehicleType)
        {
            case VehicleType.Car: vehicleSpeed = 1.0f; break;
            case VehicleType.Bus: vehicleSpeed = 0.75f; break;
            case VehicleType.Truck: vehicleSpeed = 0.5f; break;
            case VehicleType.AWESOME: vehicleSpeed = 2.0f; break;
        }
    }
}
