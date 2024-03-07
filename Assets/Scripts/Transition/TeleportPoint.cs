using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoaderEventSO loadEventSO;
    public GameSceneSO sceneToGo;       // 传送的场景
    public Vector3 positionToGo;        // 传送的位置
    
    public void TriggerAction()
    {
        //Debug.Log("success");

        loadEventSO.RaiseLoadRequest(sceneToGo, positionToGo, true);
    }
}
