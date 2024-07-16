using UnityEngine;

public class VehicleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other);

        if (!other.gameObject.TryGetComponent<VehicleBehaviour>(out VehicleBehaviour VB)) return;
        if (!VB.vehicleHandler) return;

        VB.vehicleHandler.DestroyVehicle(other.gameObject);
    }
}
