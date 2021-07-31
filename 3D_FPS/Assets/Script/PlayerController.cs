using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    //���� ����
    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool isWalk = false;
    public bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //������ üũ ����
    private Vector3 lastPos;

    //�ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        //�ʱ�ȭ
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;//������� �����̱⶧���� local(������� ����)���
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryCrouch();
        if (!Inventory.inventoryActivated)
        {
            CameraRotation();
            CharacterRotation();
        }

    }

    

    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //�ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;//����ġ ����
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;

        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }
    //�ε巯�� �ɱ� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float _PosY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_PosY != applyCrouchPosY)
        {
            count++;
            _PosY = Mathf.Lerp(_PosY, applyCrouchPosY, 0.1f);//���� �Լ� : ���������� ���� ������ �����Ѵ�.
            theCamera.transform.localPosition = new Vector3(0, _PosY, 0);
            if (count > 15)
            {
                break;
            }
            yield return null;//�������� ���
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

    }
    //���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //capsuleCollider ���� �ݸ�ŭ�� y�� ��ŭ �Ʒ��������� Ray�� ���´�. -> ���� ��´�.
        theCrosshair.JumpingAnimation(!isGround);
    }

    //���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    //����
    private void Jump()
    {
        //���� ���¿��� ���� �� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }
        theStatusController.DereaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    //�޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    //�޸��� ����
    private void Running()
    {
        //���� ���¿��� �޸��� �� ���� ���� ����
        if (isCrouch)
        {
            Crouch();
        }

        theGunController.CancleFineSight();
                   
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DereaseStamina(10);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    private void Move()
    {//�÷��̾� �̵�
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //-1, 0, 1 �� �ϳ��� ��ȯ, Ű���� �Է¿� ��� ����
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // GetAxis�� -1.0f���� 1.0f������ ���� ��ȯ, �ε巯�� �̵��� ��� 

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //���� ��� ���� 1�� �����.

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        MoveCheck();
        Move();

        TryRun();

    }

    //������ üũ
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else if(Vector3.Distance(lastPos, transform.position) < 0.01f)
            {
                isWalk = false;
            }
            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
        

    }


    //���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//���� �� �� ���̷� ����

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //�¿� �÷��̾� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _CharacterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_CharacterRotationY));

    }

}
