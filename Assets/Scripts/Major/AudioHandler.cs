using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler>
{
    [field: SerializeField] private GameObject ParentObject;
    [field: SerializeField] private List<string> AudioPaths;
    public Dictionary<string, Audible> Audibles;

    protected override void Initialize()
    {
        Audibles = new();
        List<Audible> localAudibles = new();

        foreach (string path in AudioPaths)
        {
            localAudibles.AddRange(Resources.LoadAll<Audible>(path));
        }

        foreach (Audible data in localAudibles)
        {
            AudioSource source = ParentObject.AddComponent<AudioSource>();
            source.clip = data.AudioClip;
            data.Source = source;

            bool check = string.IsNullOrWhiteSpace(data.FriendlyName) || Audibles.ContainsKey(data.FriendlyName);
            string key = (check ? data.AudioClip.name : data.FriendlyName);

            Audibles.Add(key, data);
        }
    }

    private Audible GetAudible(string clip)
    {
        Audible audible;
        bool exists = Audibles.TryGetValue(clip, out audible);

        if (!exists)
        {
            Debug.LogWarning("AudioHandler.cs | Cannot get Audible Scriptable! Perhaps the provided clip was incorrectly spelt or not used? (" + clip + ")");
            return null;
        }

        return audible;
    }

    internal bool IsPlaying(string clip)
    {
        Audible audible = GetAudible(clip);
        return audible.Source.isPlaying;
    }

    internal void Play(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Play();
    }

    internal void PlayOnce(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.PlayOneShot(audible.Source.clip);
    }

    internal void Stop(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Stop();
    }

    internal void Pause(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Pause();
    }

    internal void UnPause(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.UnPause();
    }

    // Add in Fading Here!
}
