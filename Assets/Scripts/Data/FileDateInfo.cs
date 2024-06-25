using System;
using System.IO;

[Serializable]
public class FileDateInfo
{
    public FileDateInfo(string filepath) {
        creation = File.GetCreationTime(filepath);
        modification = File.GetLastWriteTime(filepath);
    }

    public DateTime creation;
    public DateTime modification;
}