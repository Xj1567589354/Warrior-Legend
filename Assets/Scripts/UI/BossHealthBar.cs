using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image healthImage;           // 血条

    /// <summary>
    /// 接受health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
}
