using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler>
{
    [field: SerializeField] private GameObject ParentObject;
    [field: SerializeField] private List<string> AudioPaths;

    public Dictionary<string, Audible> Audibles;

    private void SourceCreation(Audible audible, Transform parent = null, GameObject gameObj = null, bool ranOnStartup = false)
    {
        if (audible.SkipStartupCreation && ranOnStartup) return;
        
        if (!gameObj)
        {
            gameObj = audible.Parent;

            if (!gameObj)
            {
                gameObj = new GameObject();
                gameObj.name = audible.name;
            }
        }

        if (!parent)
        {
            parent = gameObj.transform.parent;
            if (!parent) parent = transform;
        }

        gameObj.transform.parent = parent;

        AudioSource source = gameObj.AddComponent<AudioSource>();
        source.clip = audible.AudioClip;
        source.loop = audible.LoopAudible;
        source.playOnAwake = audible.PlayOnCreation;
        source.outputAudioMixerGroup = audible.MixerGroup;

        audible.Source = source;

        bool check = string.IsNullOrWhiteSpace(audible.FriendlyName) || Audibles.ContainsKey(audible.FriendlyName);
        string key = check ? audible.AudioClip.name : audible.FriendlyName;

        Audibles.Add(key, audible);
    }

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
            SourceCreation(data, null, null, true);
        }
    }

    public void NewSource(Audible audible, Transform parent) => SourceCreation(audible, parent);

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

    internal bool IsPlaying(Audible audible)
    {
        return audible.Source.isPlaying;
    }

    internal void Play(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Play();
    }

    internal void Play(Audible audible)
    {
        audible.Source.Play();
    }

    internal void PlayOnce(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.PlayOneShot(audible.Source.clip);
    }

    internal void PlayOnce(Audible audible)
    {
        audible.Source.PlayOneShot(audible.Source.clip);
    }

    internal void Stop(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Stop();
    }

    internal void Stop(Audible audible)
    {
        audible.Source.Stop();
    }

    internal void Pause(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.Pause();
    }

    internal void Pause(Audible audible)
    {
        audible.Source.Pause();
    }

    internal void UnPause(string clip)
    {
        Audible audible = GetAudible(clip);
        audible.Source.UnPause();
    }

    internal void UnPause(Audible audible)
    {
        audible.Source.UnPause();
    }
}
