using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Events;

public class DataHandler : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] private string DataFolder { get; set; } = "CommonData";
    [field: SerializeField] private string DataAttribute { get; set; } = ".dat";
    [field: SerializeField] private string DefaultDataFileName { get; set; } = "DataFile";
    [field: SerializeField] private bool ValidateSetupOnStartup { get; set; } = true;

    [field: Space(5)]

    [field: Header("Events")]
    [field: SerializeField] private UnityEvent ValidatedData;
    [field: SerializeField] private UnityEvent SavedToFile;
    [field: SerializeField] private UnityEvent LoadedFromFile;

    private void Awake()
    {
        if (!ValidateSetupOnStartup) return;
        if (Directory.Exists(DataFolder)) return;

        Directory.CreateDirectory(DataFolder);
        ValidatedData?.Invoke();
    }

    public void SaveToFile(SavableData data, string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            filename = DefaultDataFileName;

        BinaryFormatter formatter = new();
        FileStream dataFile = File.Create(DataFolder + "/" + filename + DataAttribute);

        formatter.Serialize(dataFile, data);
        dataFile.Close();

        SavedToFile?.Invoke();

        Debug.Log("DataHandler.cs | Successfully saved file: " + filename + " to disk. Check directory: " + DataFolder + "/ to validate!", this);
    }

    public SavableData ReadFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            filename = DefaultDataFileName;

        if (!File.Exists(DataFolder + "/" + filename + DataAttribute))
        {
            Debug.LogWarning("DataHandler.cs | Cannot retrieve data file: " + filename + DataAttribute + "as it likely doesn't exist!");
            return new SavableData();
        }

        BinaryFormatter formatter = new();
        FileStream dataFile = File.Open(DataFolder + "/" + filename + DataAttribute, FileMode.Open);

        SavableData loadedData = (SavableData)formatter.Deserialize(dataFile);
        dataFile.Close();

        LoadedFromFile?.Invoke();

        Debug.Log("DataHandler.cs | Successfully retrieved data from file: " + filename + "! (" + DataFolder + "/" + filename + DataAttribute + ")");

        return loadedData;
    }
}
