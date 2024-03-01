using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存什么数据，就需要继承此接口
/// </summary>
public interface ISaveable
{
    DataDefinition GetDataID();
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);
    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this);

    void GetSaveData(Data data);
    void LoadData(Data data);
}
