using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;

public class TweedleAudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private Routine m_Routine;

    public void SetData(AudioClip clip, float volume, float startTime)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.time = startTime;
    }

    public void Play(float waitTime)
    {
        m_Routine.Replace(this, PlayThenDestroyRoutine(waitTime));
    }

    private IEnumerator PlayThenDestroyRoutine(float waitTime)
    {
        audioSource.Play();
        if(waitTime < 0)
            yield return audioSource.WaitToComplete();
        else
            yield return waitTime;
        audioSource.Stop();
        Destroy(this.gameObject);
    }
}
