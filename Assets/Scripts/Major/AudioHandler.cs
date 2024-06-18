using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    [System.Serializable]
    struct SourceData
    {
        public string SourceName;
        public AudioMixerGroup SourceMixerGroup;
    }

    [System.Serializable]
    struct AudibleData
    {
        public Audible Audible;
        public AudioMixerGroup MixerGroup;
    }

    [field: SerializeField] private List<SourceData> Sources;
    [field: SerializeField] private List<AudibleData> Audibles;

    private void Awake()
    {
        foreach (SourceData data in Sources)
        {
            GameObject gameObject = new();
            gameObject.name = data.SourceName;
            gameObject.transform.parent = transform;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = data.SourceMixerGroup;
        }
    }

    //public void PlayAudible(AudibleData audible)
    //{

    //}

    //public void PlayAudible(string name)
    //{
    //    foreach (AudibleData data in Audibles)
    //    {
    //        if (name != data.audible.FriendlyName) continue;

    //        PlayAudible(data);
    //        break;
    //    }
    //}
}
