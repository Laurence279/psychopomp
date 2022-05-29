using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    [SerializeField] AudioSource source;

    private bool isMuted = false;

    private void Start()
    {
        StartCoroutine(PlayAudioSequentially());
    }
    IEnumerator PlayAudioSequentially()
    {
        yield return null;

        for (int i = 0; i < clips.Length; i++)
        {

            source.clip = clips[i];


            source.Play();


            while (source.isPlaying)
            {
                yield return null;
            }

        }
    }

    public void Toggle()
    {
        if(isMuted)
        {
            source.volume = 0.4f;
            isMuted = false;
            return;
        }

        source.volume = 0;
        isMuted = true;
    }
}
