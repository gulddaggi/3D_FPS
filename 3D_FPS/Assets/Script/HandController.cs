using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand cuurentHand;

    //공격중인지
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
                //코루틴 실행
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

        //공격 활성화 시점
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
        // transform.forward -> transform.TransformDirection(Vector3.forward)로도 가능 : Vector3.forward를 플레이어 기준으로 고정시킨다.
        {
            return true;
        }
        return false;

    }

}
