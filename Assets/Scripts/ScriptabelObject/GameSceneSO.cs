using UnityEngine;
using UnityEngine.AddressableAssets;


// ��Ҫ���ص�Assets
[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public AssetReference assetReference;       // ������Դ����
    public Scenetype scenetype;                 // ��������
}
