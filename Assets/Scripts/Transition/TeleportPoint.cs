using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Iinteractable
{
    public SceneLoaderEventSO loadEventSO;
    public GameSceneSO sceneToGo;       // ���͵ĳ���
    public Vector3 positionToGo;        // ���͵�λ��

    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequest(sceneToGo, positionToGo, true);
    }
}
