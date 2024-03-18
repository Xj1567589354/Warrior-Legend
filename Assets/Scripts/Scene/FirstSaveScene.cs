using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSaveScene : MonoBehaviour
{
    public VoidEventSO saveDataEvent;

    private void Awake()
    {
        StartCoroutine(SaveCaveScene());
    }

    public IEnumerator SaveCaveScene()
    {

        yield return new WaitForSeconds(0.1f);

        if (DataManager.instance.count == 0)
        {
            saveDataEvent.RasieEvent();     // ±£´æÊý¾Ý
            print("Save!!!");
        }
    }
}
