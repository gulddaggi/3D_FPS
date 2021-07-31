using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //충돌한 오브젝트의 컬라이더를 저장하는 리스트
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; //지상 레이어
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
        foreach(Transform tf_Child in this.transform) //자신과 자식객체들의 transform을 가져와 반복문
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length]; //inspector에 있는 정보 받아옴

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)//제외시킨 레이어들 예외처리
        {
            colliderList.Add(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)//제외시킨 레이어들 예외처리
        {
            colliderList.Remove(other);

        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
