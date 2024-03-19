using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour, ISaveable
{
    public Image healthImage;           // 血条
    public Image healthDelayImage;      // 延迟红条
    public Image powerImage;            // 能量条

    public float delaySpeed = 3;           // 延迟速度
    public bool isRecovering;       // 恢复状态

    private Charactor currentCharactor;     // 当前角色

    public int startCoinQuantity;
    public int startKeyQuantity;
    public TextMeshProUGUI coinQuantity;
    public TextMeshProUGUI keyQuantity;

    public static int currentCoinQuantity;
    public static int currentKeyQuantity;

    private void OnEnable()
    {
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void Start()
    {
        currentCoinQuantity = startCoinQuantity;
        currentKeyQuantity = startKeyQuantity;
    }

    private void Update()
    {
        coinQuantity.text = currentCoinQuantity.ToString();
        keyQuantity.text = currentKeyQuantity.ToString();

        if(healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * delaySpeed;
        }

        // 恢复
        if (isRecovering)
        {
            float persentage = currentCharactor.currentPower / currentCharactor.maxPower;
            powerImage.fillAmount = persentage;

            if(persentage >= 1)
            {
                isRecovering = false;
                return;
            }
        }
    }

    /// <summary>
    /// 接受health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }

    /// <summary>
    /// 接受能量条的变更
    /// </summary>
    /// <param name="charactor"></param>
    public void OnPoerChange(Charactor charactor)
    {
        isRecovering = true;
        // 获取当前角色
        currentCharactor = charactor;       
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.intSaveDataDict.ContainsKey(GetDataID().ID + "Coin")
            && data.intSaveDataDict.ContainsKey(GetDataID().ID + "Key"))
        {
            data.intSaveDataDict[GetDataID().ID + "Coin"] = currentCoinQuantity;
            data.intSaveDataDict[GetDataID().ID + "Key"] = currentKeyQuantity;
        }
        else
        {
            data.intSaveDataDict.Add(GetDataID().ID + "Coin", currentCoinQuantity);
            data.intSaveDataDict.Add(GetDataID().ID + "Key", currentKeyQuantity);
        }
    }

    public void LoadData(Data data)
    {
        if (data.intSaveDataDict.ContainsKey(GetDataID().ID + "Coin")
            && data.intSaveDataDict.ContainsKey(GetDataID().ID + "Key"))
        {
            currentCoinQuantity = data.intSaveDataDict[GetDataID().ID + "Coin"];
            currentKeyQuantity = data.intSaveDataDict[(GetDataID().ID) + "Key"];
        }
    }
}
