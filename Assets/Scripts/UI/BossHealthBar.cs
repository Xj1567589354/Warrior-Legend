using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Image healthImage;           // 血条
    public TextMeshProUGUI healthNums;      // 血条值
    public Charactor boss;

    private void Start()
    {
        healthNums.text = boss.currentHealth.ToString();
    }

    /// <summary>
    /// 接受health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：Current/Max</param>
    public void OnHealthChange(float persentage, float healthnumber)
    {
        healthImage.fillAmount = persentage;
        healthNums.text = healthnumber.ToString();
    }
}
