using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audible", menuName = "Scriptables/Audio Handler", order = 1)]
public class Audible : ScriptableObject
{
    public string FriendlyName { get; set; }
    public AudioClip AudioClip { get; set; }
    public AudioMixerGroup MixerGroup { get; set; }

    [field: Space(2.5f)]

    public GameObject Parent { get; set; }

    internal AudioSource Source;
}
