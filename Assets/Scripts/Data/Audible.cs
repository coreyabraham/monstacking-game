using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audible", menuName = "Scriptables/Audio Handler", order = 1)]
public class Audible : ScriptableObject
{
    [field: SerializeField] public string FriendlyName { get; set; }
    [field: SerializeField] public AudioClip AudioClip { get; set; }
    [field: SerializeField] public AudioMixerGroup MixerGroup { get; set; }

    [field: Space(2.5f)]

    [field: SerializeField] public GameObject Parent { get; set; }

    internal AudioSource Source;
}
