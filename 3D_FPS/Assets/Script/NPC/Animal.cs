using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected string animalName; //������ �̸�
    [SerializeField]
    protected int hp; //������ ü��
    [SerializeField]
    protected float walkSpeed; //�ȱ� ���ǵ�
    [SerializeField]
    protected float runSpeed; //�ٱ� ���ǵ�
    [SerializeField]
    protected float turningSpeed; // ȸ�� ���ǵ�
    protected float applySpeed;


    protected Vector3 direction; //����

    //���º���
    protected bool isWalking; //�ȴ������� �ƴ��� �Ǻ�
    protected bool isAction; //�ൿ������ �ƴ��� �Ǻ�
    protected bool isRunning; //�ٴ��� �Ǻ�
    protected bool isDead; //�׾����� �Ǻ�



    [SerializeField]
    protected float walkTime; //�ȱ� �ð�
    [SerializeField]
    protected float waitTime; //��� �ð�
    [SerializeField]
    protected float runTime; //�ٱ� �ð�
    protected float currentTime;

    //�ʿ��� ������Ʈ
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
        int _random = Random.Range(0, 3); //�ϻ� ���� 3��
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
