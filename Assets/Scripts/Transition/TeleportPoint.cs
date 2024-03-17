using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoaderEventSO loadEventSO;
    public GameSceneSO sceneToGo;       // 传送的场景
    public Vector3 positionToGo;        // 传送的位置
    public VoidEventSO saveDataEvent;

    public bool isTeleEnable;       // 是否开启传送点开关
    public GameObject telepoint;

    public void TriggerAction()
    {
        //Debug.Log("success");

        loadEventSO.RaiseLoadRequest(sceneToGo, positionToGo, true);
    }

    private void Awake()
    {
        StartCoroutine(SaveCaveScene());
    }
    //private void Update()
    //{
    //    if (isTeleEnable)
    //    {
    //        if (coinNums >= 5 && this.gameObject.tag == "Untagged")
    //        {
    //            this.gameObject.tag = "Interactable";
    //            print("s");
    //        }
    //    }
    //}


    public IEnumerator SaveCaveScene()
    {

        yield return new WaitForSeconds(0.1f);

        if (DataManager.instance.count == 0)
        {
            saveDataEvent.RasieEvent();     // 保存数据
            print("Save!!!");
        }
    }

    //private void GetCoinNums(int _coinNums)
    //{
    //    this.coinNums = _coinNums;
    //}
}
