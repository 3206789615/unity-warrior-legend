using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeCanvas : MonoBehaviour
{
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    private void OnFadeEvent(Color target,float duration)
    {
        fadeImage.DOBlendableColor(target, duration);
    }

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, duration);
    }
}
