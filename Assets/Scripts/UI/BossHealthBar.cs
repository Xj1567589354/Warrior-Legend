using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Image healthImage;           // Ѫ��
    public TextMeshProUGUI healthNums;      // Ѫ��ֵ
    public Charactor boss;

    private void Start()
    {
        healthNums.text = boss.currentHealth.ToString();
    }

    /// <summary>
    /// ����health�ı���ٷֱ�
    /// </summary>
    /// <param name="persentage">�ٷֱȣ�Current/Max</param>
    public void OnHealthChange(float persentage, float healthnumber)
    {
        healthImage.fillAmount = persentage;
        healthNums.text = healthnumber.ToString();
    }
}
