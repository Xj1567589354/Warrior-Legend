using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoad : MonoBehaviour
{
    public GameObject gameObject;
    public SceneLoader sceneLoader;

    public GameSceneSO currentGameScene;

    [Header("资产类型")]
    public GameSceneSO Foreast;
    public GameSceneSO Cave;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        currentGameScene = sceneLoader.currentLoadedScene;

        if (currentGameScene == Foreast)
        {
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
}
