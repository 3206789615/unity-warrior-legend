using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;
    public Vector3 firstposition;
    public Vector3 menuPosition;
    public SceneLoadEventSO loadEventSO;
    public SceneLoadEventSO unLoadedSceneEvent;
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScreen;

    private bool isLoading;

    public float fadeDuration;
    public VoidEvent afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public VoidEvent newGameEvent;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);        
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstposition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstposition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;

        isLoading = true;
        sceneToLoad = locationToGo;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene!=null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
       
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        unLoadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        yield return  currentLoadedScene.sceneReference.UnLoadScene();
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();       
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;

        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
             fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        if(currentLoadedScene.sceneType==SceneType.Location)
            afterSceneLoadedEvent.RaiseEvent();
    }
}
