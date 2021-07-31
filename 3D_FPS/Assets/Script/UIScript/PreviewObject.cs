using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //�浹�� ������Ʈ�� �ö��̴��� �����ϴ� ����Ʈ
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; //���� ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    void Start()
    {
        
    }

    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform tf_Child in this.transform) //�ڽŰ� �ڽİ�ü���� transform�� ������ �ݺ���
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length]; //inspector�� �ִ� ���� �޾ƿ�

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)//���ܽ�Ų ���̾�� ����ó��
        {
            colliderList.Add(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)//���ܽ�Ų ���̾�� ����ó��
        {
            colliderList.Remove(other);

        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
