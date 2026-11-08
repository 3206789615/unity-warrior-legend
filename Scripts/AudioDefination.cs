using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEvent PlayAudioEvent;
    public AudioClip audioClip;
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()
    {
        PlayAudioEvent.RaiseEvent(audioClip);
    }
}
