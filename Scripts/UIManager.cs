using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public PlayerStatBar playerStatBar;
    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEvent healthEvent;

    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnhealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }
  

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnhealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
            playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnhealthEvent(Character character)
    {
        var presentage= character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(presentage);
    }
}
