using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //플레이어의 움직임 제어
    public static bool isOpenInventory = false; //인벤토리 활성화
    public static bool isOpenCraftManual = false; //건축 메뉴창 활성화

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서 잠금
        Cursor.visible = false; //커서가 안보임
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
    }
}
