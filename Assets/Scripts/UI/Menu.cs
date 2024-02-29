using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameButton;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);       // 设置首选目标
    }

    public void OnExitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }
}
