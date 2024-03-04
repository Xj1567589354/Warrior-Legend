using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    /*
     ��Ҫ���������
    ��ɫ����
    ����float��ֵ���ø��ֵ䱣��--�����ɫѪ��
    ����
     */
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>(); 
    public Dictionary<string, float> floatSaveDataDict  = new Dictionary<string, float>();
    public Dictionary<string, bool> boolSaveDataDict = new Dictionary<string, bool>();

    public string sceneToLoad;      // ��Ҫ�����string���ͳ�����

    /*����ģʽ*/

    /// <summary>
    /// ��SceneתΪString����
    /// </summary>
    /// <param name="gameSceneSO"></param>
    public void SaveGameSceneToString(GameSceneSO saveScene)
    {
        sceneToLoad = JsonUtility.ToJson(saveScene);      // ��object����ת��Ϊstring����
        Debug.Log(sceneToLoad);
    }

    /// <summary>
    /// ��Sceneת��ΪObject����
    /// </summary>
    /// <returns></returns>
    public GameSceneSO GetSavedSceneToObject()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();      // ����һ��object���Ͷ���
        JsonUtility.FromJsonOverwrite(sceneToLoad, newScene);               // ��string����scneToLoadת��Ϊobject����newScene

        return newScene;
    }
}
