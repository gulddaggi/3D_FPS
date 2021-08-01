using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float secondPerRealTimeSecond; //���� ������ 100�� = ���� ������ 1��


    [SerializeField]
    private float fogDensityScale;//������ ����

    [SerializeField]
    private float nightFogDensity; //�� ������ fog�е�
    private float dayFogDensity; //�� ������ fog�е�
    private float currentFogDensity; //���
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
        if (transform.eulerAngles.x >= 170)
        {
           GameManager.isNight = true;
        }
        else if (transform.eulerAngles.x >= 350)
        {
            GameManager.isNight = false;
        }

        if (GameManager.isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityScale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityScale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
