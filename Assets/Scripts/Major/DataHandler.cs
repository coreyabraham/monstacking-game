using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Events;
using System.Drawing;

public class DataHandler : Singleton<DataHandler>
{
    [System.Serializable]
    public class EventCollection
    {
        public UnityEvent ValidatedData;
        public UnityEvent SavedToFile;
        public UnityEvent LoadedFromFile;
        public UnityEvent CachedDataUpdated;
    }

    [field: Header("Settings")]
    [field: SerializeField] private string DataFolder { get; set; } = "CommonData";
    [field: SerializeField] private string DataAttribute { get; set; } = ".dat";
    [field: SerializeField] private string DefaultDataUserName { get; set; } = "DataFile";

    [field: Space(0.5f)]

    [field: SerializeField] private bool ValidateSetupOnStartup { get; set; } = true;
    [field: SerializeField] private bool DisableDebugOutput { get; set; } = true;

    [field: Header("Others")]
    public EventCollection Events;
    [field: SerializeField] public List<SavableData> CachedData = new();

    protected override void Initialize()
    {
        if (!ValidateSetupOnStartup) return;
        
        UpdateCachedFiles();
        
        if (Directory.Exists(DataFolder)) return;

        Directory.CreateDirectory(DataFolder);
        Events.ValidatedData?.Invoke();
    }

    private string GetFolderDirectory() => Directory.GetCurrentDirectory() + "/" + DataFolder;

    private string ResolveDuplicatedUserName(string username)
    {
        int amountOfIdenticalNames = 0;

        foreach (SavableData readData in CachedData)
        {
            if (readData.name != username) continue;
            amountOfIdenticalNames++;
        }

        print(amountOfIdenticalNames);
        print(amountOfIdenticalNames > 1);

        print(username.Contains(" (" + amountOfIdenticalNames.ToString() + ")"));

        return (amountOfIdenticalNames > 0) ? (username + " (" + amountOfIdenticalNames.ToString() + ")") : username;
    }

    public void SaveToFile(SavableData data, string username, bool updateCache = true)
    {
        if (string.IsNullOrWhiteSpace(username))
            username = DefaultDataUserName;

        data.name = ResolveDuplicatedUserName(username);
        username = data.name;
        
        BinaryFormatter formatter = new();
        FileStream dataFile = File.Create(DataFolder + "/" + username + DataAttribute);

        formatter.Serialize(dataFile, data);
        dataFile.Close();

        Events.SavedToFile?.Invoke();

        if (!DisableDebugOutput) Debug.Log("DataHandler.cs | Successfully saved file: " + username + " to disk. Check directory: " + GetFolderDirectory() + "/ to validate!", this);
        if (updateCache) UpdateCachedFiles();
    }

    public SavableData ReadFromFile(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            username = DefaultDataUserName;

        if (!File.Exists(DataFolder + "/" + username + DataAttribute))
        {
            if (!DisableDebugOutput) Debug.LogWarning("DataHandler.cs | Cannot retrieve data file: " + username + DataAttribute + "as it likely doesn't exist!");
            return new SavableData();
        }

        BinaryFormatter formatter = new();
        FileStream dataFile = File.Open(DataFolder + "/" + username + DataAttribute, FileMode.Open);

        SavableData loadedData = (SavableData)formatter.Deserialize(dataFile);
        dataFile.Close();

        Events.LoadedFromFile?.Invoke();

        if (!DisableDebugOutput) Debug.Log("DataHandler.cs | Successfully retrieved data from file: " + username + "! (" + GetFolderDirectory() + "/" + username + DataAttribute + ")");

        return loadedData;
    }

    public void UpdateCachedFiles()
    {
        if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);

        string[] files = Directory.GetFiles(DataFolder);
        List<SavableData> allDataObjects = new();

        foreach (string str in files)
        {
            string step1 = str.Remove(str.Length - DataAttribute.Length, DataAttribute.Length);
            string final = step1.Remove(0, DataFolder.Length + 1);

            SavableData data = ReadFromFile(final);

            if (string.IsNullOrEmpty(data.name))
            {
                if (!DisableDebugOutput) Debug.LogWarning("Data from File: " + str + " Cannot be read from!");
                continue;
            }

            allDataObjects.Add(data);
        }

        CachedData = allDataObjects;
        Events.CachedDataUpdated?.Invoke();
    }
}
