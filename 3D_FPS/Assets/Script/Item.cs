using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="New Item/item")]//����â�� ���ο� ���� ���
public class Item : ScriptableObject //���ӿ�����Ʈ�� �Ҵ��� �ʿ䰡 ����.
{
    public string itemName; //������ �̸�
    public ItemType itemType; //������ ����
    public Sprite itemImage; //������ �̹���
    public GameObject itemPrefab; //������ ������

    public string weqponType;//���� ����

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }


}
