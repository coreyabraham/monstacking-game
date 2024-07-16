using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleHandler : MonoBehaviour
{
    [System.Serializable]
    public struct VehicleSpawner
    {
        public GameObject PointA;
        public GameObject PointB;
    }

    [field: Header("Assets")]
    [field: SerializeField] private GameObject VehicleHolder;
    [field: SerializeField] private XRInteractionManager VRInteractManager;

    [field: Space(2.5f)]

    [field: SerializeField] private List<VehicleSpawner> Spawns;
    [field: SerializeField] private List<GameObject> Models;

    [field: Header("Settings")]
    [field: SerializeField] private float IntervalBetweenSpawns { get; set; } = 1.0f;

    [field: SerializeField] private int VehicleSpawnCap = 10;
    [field: SerializeField] private float VehicleSpeed = 1.0f;

    [field: Header("Miscellaneous")]
    [field: SerializeField] private List<Audible> Sounds;
    [field: SerializeField] private List<Audible> Voices;

    private float timeInterval = 0.0f;
    private int vehiclesSpawned = 0;

    private void SpawnVehicle()
    {
        if (vehiclesSpawned > VehicleSpawnCap)
        {
            Debug.LogWarning("The maximum vehicles that can be spawned have been reached! (" + VehicleSpawnCap.ToString() + ")", this);
            return;
        }

        int modelIndex = Random.Range(0, Models.Count);
        GameObject ModelPointer = Models[modelIndex];

        if (!ModelPointer)
        {
            Debug.LogWarning("Error Getting Model in `List<GameObject> Models` Models with Integer Index: " + modelIndex.ToString(), this);
            return;
        }

        int positionIndex = Random.Range(0, Spawns.Count);
        VehicleSpawner SpawnPointer = Spawns[positionIndex];

        SpawnPointer.PointA.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

        GameObject obj = Instantiate(ModelPointer);
        obj.transform.parent = VehicleHolder.transform;
        obj.transform.SetPositionAndRotation(position, rotation);

        if (!obj.TryGetComponent(out VehicleBehaviour VB))
            VB = obj.AddComponent<VehicleBehaviour>();

        VB.vehicleHandler = this;

        if (Voices.Count != 0)
        {
            int voiceIndex = Random.Range(0, Voices.Count);
            Audible voice = Voices[voiceIndex];
            VB.vehicleVoice = voice;
        }

        VB.vehicleDestination = SpawnPointer.PointB.transform.position;
        VB.vehicleSpeed = VehicleSpeed;

        if (!obj.TryGetComponent(out XRGrabInteractable GI))
            GI = obj.AddComponent<XRGrabInteractable>();

        GI.interactionManager = VRInteractManager;

        vehiclesSpawned++;
    }

    public void DestroyVehicle(GameObject Vehicle)
    {
        if (!Vehicle.TryGetComponent(out VehicleBehaviour VB)) return;
        Destroy(Vehicle);
        vehiclesSpawned--;
    }

    private void Update()
    {
        if (!GameHandler.Instance.IsGameRunning()) return;

        if (timeInterval < IntervalBetweenSpawns)
        {
            timeInterval += 0.1f * Time.deltaTime;
            return;
        }

        timeInterval = 0.0f;
        SpawnVehicle();
    }
}
