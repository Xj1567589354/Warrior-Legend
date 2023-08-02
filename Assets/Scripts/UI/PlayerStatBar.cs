using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;           // Ѫ��
    public Image healthDelayImage;      // �ӳٺ���
    public Image powerImage;            // ������

    public float delaySpeed = 3;           // �ӳ��ٶ�

    public bool isRecovering;       // �ָ�״̬

    private Charactor currentCharactor;     // ��ǰ��ɫ


    private void Update()
    {
        if(healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * delaySpeed;
        }

        // �ָ�
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
    /// ����health�ı���ٷֱ�
    /// </summary>
    /// <param name="persentage">�ٷֱȣ�Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }

    /// <summary>
    /// �����������ı��
    /// </summary>
    /// <param name="charactor"></param>
    public void OnPoerChange(Charactor charactor)
    {
        isRecovering = true;
        // ��ȡ��ǰ��ɫ
        currentCharactor = charactor;       
    }
}
