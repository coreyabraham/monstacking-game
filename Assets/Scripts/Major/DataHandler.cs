using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Events;

public class DataHandler : Singleton<DataHandler>
{
    [field: Header("Settings")]
    [field: SerializeField] private string DataFolder { get; set; } = "CommonData";
    [field: SerializeField] private string DataAttribute { get; set; } = ".dat";
    [field: SerializeField] private string DefaultDataFileName { get; set; } = "DataFile";

    [field: Space(0.5f)]

    [field: SerializeField] private bool ValidateSetupOnStartup { get; set; } = true;
    [field: SerializeField] private bool DisableDebugOutput { get; set; } = true;

    [field: Header("Events")]
    [field: SerializeField] public UnityEvent ValidatedData;
    [field: SerializeField] public UnityEvent SavedToFile;
    [field: SerializeField] public UnityEvent LoadedFromFile;

    protected override void Initialize()
    {
        if (!ValidateSetupOnStartup) return;
        if (Directory.Exists(DataFolder)) return;

        Directory.CreateDirectory(DataFolder);
        ValidatedData?.Invoke();
    }

    private string GetFolderDirectory() => Directory.GetCurrentDirectory() + "/" + DataFolder;

    public void SaveToFile(SavableData data, string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            filename = DefaultDataFileName;

        BinaryFormatter formatter = new();
        FileStream dataFile = File.Create(DataFolder + "/" + filename + DataAttribute);

        formatter.Serialize(dataFile, data);
        dataFile.Close();

        SavedToFile?.Invoke();

        if (!DisableDebugOutput) Debug.Log("DataHandler.cs | Successfully saved file: " + filename + " to disk. Check directory: " + GetFolderDirectory() + "/ to validate!", this);
    }

    public SavableData ReadFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            filename = DefaultDataFileName;

        if (!File.Exists(DataFolder + "/" + filename + DataAttribute))
        {
            if (!DisableDebugOutput) Debug.LogWarning("DataHandler.cs | Cannot retrieve data file: " + filename + DataAttribute + "as it likely doesn't exist!");
            return new SavableData();
        }

        BinaryFormatter formatter = new();
        FileStream dataFile = File.Open(DataFolder + "/" + filename + DataAttribute, FileMode.Open);

        SavableData loadedData = (SavableData)formatter.Deserialize(dataFile);
        dataFile.Close();

        LoadedFromFile?.Invoke();

        if (!DisableDebugOutput) Debug.Log("DataHandler.cs | Successfully retrieved data from file: " + filename + "! (" + GetFolderDirectory() + "/" + filename + DataAttribute + ")");

        return loadedData;
    }

    public List<SavableData> ReadFromMultipleFiles()
    {
        string[] files = Directory.GetFiles(DataFolder);
        List<SavableData> allDatAObjects = new();

        foreach (string str in files)
        {
            string step1 = str.Remove(str.Length - DataAttribute.Length, DataAttribute.Length);
            // print("Step1: " + step1);

            string step2 = step1.Remove(0, DataFolder.Length + 1);
            // print("Step2: " + step2);

            SavableData data = ReadFromFile(step2);

            if (string.IsNullOrEmpty(data.name))
            {
                if (!DisableDebugOutput) Debug.LogWarning("Data from File: " + str + " Cannot be read from!");
                continue;
            }

            allDatAObjects.Add(data);
        }

        return allDatAObjects;
    }
}
