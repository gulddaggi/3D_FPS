using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //�÷��̾��� ������ ����
    public static bool isOpenInventory = false; //�κ��丮 Ȱ��ȭ
    public static bool isOpenCraftManual = false; //���� �޴�â Ȱ��ȭ

    public static bool isNight = false;
    public static bool isWater = false;

    private WeaponManager theWM;
    private bool flag = false;

    void Start()
    {
        theWM = FindObjectOfType<WeaponManager>();
        Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ���
        Cursor.visible = false; //Ŀ���� �Ⱥ���
    }

    void Update()
    {
        if (isOpenInventory || isOpenCraftManual)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                theWM.WeaponOut();
                flag = false;
            }
        }
    }
}
