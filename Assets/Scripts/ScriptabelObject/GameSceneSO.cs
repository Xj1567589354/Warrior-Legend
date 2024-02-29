using UnityEngine;
using UnityEngine.AddressableAssets;


// 需要加载的Assets
[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public AssetReference assetReference;       // 加载资源引用
    public Scenetype scenetype;                 // 场景类型
}
