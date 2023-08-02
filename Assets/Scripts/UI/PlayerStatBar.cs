using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;           // 血条
    public Image healthDelayImage;      // 延迟红条
    public Image powerImage;            // 能量条

    public float delaySpeed = 3;           // 延迟速度

    public bool isRecovering;       // 恢复状态

    private Charactor currentCharactor;     // 当前角色


    private void Update()
    {
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
}
