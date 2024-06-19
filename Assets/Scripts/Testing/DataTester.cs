using System.Collections.Generic;
using UnityEngine;

public class DataTester : MonoBehaviour
{
    public SavableData testData;

    private void Callback()
    {
        List<SavableData> results = DataHandler.Instance.ReadFromMultipleFiles();

        foreach (SavableData data in results)
        {
            print(data.name);
        }
    }

    private void Start()
    {
        DataHandler.Instance.SavedToFile.AddListener(Callback);
        DataHandler.Instance.SaveToFile(testData, testData.name);
    }
}
