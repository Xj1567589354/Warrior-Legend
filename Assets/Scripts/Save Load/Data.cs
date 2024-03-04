using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    /*
     需要保存的数据
    角色坐标
    所有float的值都用该字典保存--比如角色血量
    场景
     */
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>(); 
    public Dictionary<string, float> floatSaveDataDict  = new Dictionary<string, float>();
    public Dictionary<string, bool> boolSaveDataDict = new Dictionary<string, bool>();

    public string sceneToLoad;      // 需要保存的string类型场景名

    /*工厂模式*/

    /// <summary>
    /// 将Scene转为String类型
    /// </summary>
    /// <param name="gameSceneSO"></param>
    public void SaveGameSceneToString(GameSceneSO saveScene)
    {
        sceneToLoad = JsonUtility.ToJson(saveScene);      // 将object类型转换为string类型
        Debug.Log(sceneToLoad);
    }

    /// <summary>
    /// 将Scene转换为Object类型
    /// </summary>
    /// <returns></returns>
    public GameSceneSO GetSavedSceneToObject()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();      // 创建一个object类型对象
        JsonUtility.FromJsonOverwrite(sceneToLoad, newScene);               // 将string类型scneToLoad转换为object类型newScene

        return newScene;
    }
}
