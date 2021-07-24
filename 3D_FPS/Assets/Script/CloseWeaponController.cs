using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour //미완성 클래스 = 추상 클래스, 컴포넌트로 추가 불가능 -> update함수 실행 x
{

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //공격중인지
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;




    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //코루틴 실행
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

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine(); //미완성 = 추상 코루틴, 자식 클래스에서 완성


    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        // transform.forward -> transform.TransformDirection(Vector3.forward)로도 가능 : Vector3.forward를 플레이어 기준으로 고정시킨다.
        {
            return true;
        }
        return false;

    }

    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon) //virtual : 가상함수. 완성함수이지만 추가 편집가능한 함수
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false); // 오브젝트를 사라지게 만든다.
        }

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); //모든 객체에는 Transform이 존재한다.
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);

    }
}
