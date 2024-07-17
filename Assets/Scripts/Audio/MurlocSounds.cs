using System.Collections.Generic;
using UnityEngine;

public class MurlocSounds: MonoBehaviour
{
    [field: SerializeField] private Vector2 RandomizedMaxTime = new Vector2(5.0f, 10.0f);
    [field: SerializeField] private List<Audible> Voices;

    private float ChosenValue = 0.0f;
    private bool DictatedValue = false;

    private float CurrentTime;

    private void Start()
    {
        foreach (Audible audible in Voices)
        {
            AudioHandler.Instance.NewSource(audible, transform);
        }
    }

    private void Update()
    {
        if (!GameHandler.Instance.IsGameRunning()) return;

        if (!DictatedValue)
        {
            ChosenValue = Random.Range(RandomizedMaxTime.x, RandomizedMaxTime.y);
            DictatedValue = true;
        }

        if (CurrentTime < ChosenValue)
        {
            CurrentTime += Time.deltaTime;
            return;
        }

        CurrentTime = 0.0f;
        DictatedValue = false;

        AudioHandler.Instance.Play(Voices[Random.Range(0, Voices.Count)]);
    }
}
