using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = false;


    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    private Hand currentHand;

    //����������
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    void Update()
    {
        if (isActivate)
        {
            TryAttack();

        }

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
        currentHand.anim.SetTrigger("Attack");
        
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
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
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        // transform.forward -> transform.TransformDirection(Vector3.forward)�ε� ���� : Vector3.forward�� �÷��̾� �������� ������Ų��.
        {
            return true;
        }
        return false;

    }

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false); // ������Ʈ�� ������� �����.
        }

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>(); //��� ��ü���� Transform�� �����Ѵ�.
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;

    }

}
