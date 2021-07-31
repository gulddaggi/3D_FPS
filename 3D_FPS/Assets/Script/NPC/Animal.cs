using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected string animalName; //동물의 이름
    [SerializeField]
    protected int hp; //동물의 체력
    [SerializeField]
    protected float walkSpeed; //걷기 스피드
    [SerializeField]
    protected float runSpeed; //뛰기 스피드
    [SerializeField]
    protected float turningSpeed; // 회전 스피드
    protected float applySpeed;


    protected Vector3 direction; //방향

    //상태변수
    protected bool isWalking; //걷는중인지 아닌지 판별
    protected bool isAction; //행동중인지 아닌지 판별
    protected bool isRunning; //뛰는지 판별
    protected bool isDead; //죽었는지 판별



    [SerializeField]
    protected float walkTime; //걷기 시간
    [SerializeField]
    protected float waitTime; //대기 시간
    [SerializeField]
    protected float runTime; //뛰기 시간
    protected float currentTime;

    //필요한 컴포넌트
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource theAudio;

    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip soung_Hurt;
    [SerializeField]
    protected AudioClip soung_Dead;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {


    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    protected void ElapseTime()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ReSet();
        }
    }

    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0, 360f), 0f);
    }




    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
    }

   

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(soung_Hurt);
            anim.SetTrigger("Hurt");
        }

    }

    protected void Dead()
    {
        PlaySE(soung_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
