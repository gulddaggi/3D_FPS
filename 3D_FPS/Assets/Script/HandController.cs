using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    private Hand cuurentHand;

    //����������
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    void Update()
    {
        TryAttack();
        
    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        cuurentHand.anim.SetTrigger("Attack");
        
        yield return new WaitForSeconds(cuurentHand.attackDelayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(cuurentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(cuurentHand.attackDelay - cuurentHand.attackDelayA - cuurentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, cuurentHand.range))
        // transform.forward -> transform.TransformDirection(Vector3.forward)�ε� ���� : Vector3.forward�� �÷��̾� �������� ������Ų��.
        {
            return true;
        }
        return false;

    }

}
