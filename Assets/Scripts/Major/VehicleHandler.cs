using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// TODO: Add Box / Mesh Colliders on ALL Prefabs!

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

    [field: Header("Settings - Spawning")]
    [field: SerializeField] private float IntervalBetweenSpawns { get; set; } = 1.0f;

    private float timeInterval = 0.0f;

    private void SpawnVehicle()
    {
        int modelIndex = Random.Range(0, Models.Count);
        GameObject ModelPointer = Models[modelIndex];

        if (!ModelPointer)
        {
            Debug.LogError("Error Getting Model in `List<GameObject> Models` Models with Integer Index: " + modelIndex.ToString(), this);
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

        VB.vehicleDestination = SpawnPointer.PointB.transform.position;

        if (!obj.TryGetComponent(out XRGrabInteractable GI))
            GI = obj.AddComponent<XRGrabInteractable>();

        GI.interactionManager = VRInteractManager;
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
