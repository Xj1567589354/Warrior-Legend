using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����������壬����ÿ�����˵�Ψһ��GUID
public class DataDefinition : MonoBehaviour
{
    public PersistentType persistentType;
    public string ID;

    private void OnValidate()
    {
        if (persistentType == PersistentType.ReadWrite)
        {
            if (ID == string.Empty)
                ID = System.Guid.NewGuid().ToString();      // �Զ�����GUID
        }
        else
            ID = string.Empty;
    }
}
