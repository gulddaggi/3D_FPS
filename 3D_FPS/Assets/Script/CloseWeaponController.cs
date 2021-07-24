using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour //�̿ϼ� Ŭ���� = �߻� Ŭ����, ������Ʈ�� �߰� �Ұ��� -> update�Լ� ���� x
{

    //���� ������ Hand�� Ÿ�� ����
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //����������
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;




    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine(); //�̿ϼ� = �߻� �ڷ�ƾ, �ڽ� Ŭ�������� �ϼ�


    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        // transform.forward -> transform.TransformDirection(Vector3.forward)�ε� ���� : Vector3.forward�� �÷��̾� �������� ������Ų��.
        {
            return true;
        }
        return false;

    }

    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon) //virtual : �����Լ�. �ϼ��Լ������� �߰� ���������� �Լ�
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false); // ������Ʈ�� ������� �����.
        }

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); //��� ��ü���� Transform�� �����Ѵ�.
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);

    }
}
