using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="New Item/item")]//생성창에 새로운 파일 등록
public class Item : ScriptableObject //게임오브젝트에 할당할 필요가 없다.
{
    public string itemName; //아이템 이름
    public ItemType itemType; //아이템 유형
    public Sprite itemImage; //아이템 이미지
    public GameObject itemPrefab; //아이템 프리팹

    public string weqponType;//무기 유형

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }


}
