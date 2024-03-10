using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoaderEventSO loadEventSO;
    public GameSceneSO sceneToGo;       // 传送的场景
    public Vector3 positionToGo;        // 传送的位置
    public VoidEventSO saveDataEvent;

    public void TriggerAction()
    {
        //Debug.Log("success");

        loadEventSO.RaiseLoadRequest(sceneToGo, positionToGo, true);
    }

    private void Awake()
    {
        StartCoroutine(SaveCaveScene());
    }

    public IEnumerator SaveCaveScene()
    {

        yield return new WaitForSeconds(0.1f);

        if (DataManager.instance.count == 0)
        {
            saveDataEvent.RasieEvent();     // 保存数据
            print("Save!!!");
        }
    }
}
