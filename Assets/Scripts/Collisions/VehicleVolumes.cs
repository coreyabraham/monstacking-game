using System.Collections.Generic;
using UnityEngine;

public class VehicleVolumes : MonoBehaviour
{
    private List<VehicleBehaviour> Vehicles = new();

    private VehicleBehaviour GetVehicleBehaviour(Collider other) => other.gameObject.GetComponent<VehicleBehaviour>();

    private void OnTriggerEnter(Collider other)
    {
        VehicleBehaviour VB = GetVehicleBehaviour(other);
        if (!VB) return;

        VB.insideVolume = true;
        VB.ResetTimer(false);
        Vehicles.Add(VB);
    }

    private void OnTriggerExit(Collider other)
    {
        VehicleBehaviour VB = GetVehicleBehaviour(other);
        if (!VB) return;

        VB.insideVolume = false;
        VB.ResetTimer(true);
        Vehicles.Remove(VB);
    }

    private void Update()
    {
        GameHandler.Instance.SetPlayerScore(Vehicles.Count);
    }
}
