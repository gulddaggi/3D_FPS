using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조성 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    //점프 변수
    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isWalk = false;
    public bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //움직임 체크 변수
    private Vector3 lastPos;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //필요한 컴포넌트
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

        //초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;//상대적인 기준이기때문에 local(상대적인 변수)사용
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

    

    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;//스위치 역할
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
    //부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine()
    {
        float _PosY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_PosY != applyCrouchPosY)
        {
            count++;
            _PosY = Mathf.Lerp(_PosY, applyCrouchPosY, 0.1f);//보간 함수 : 목적지까지 일정 비율로 증가한다.
            theCamera.transform.localPosition = new Vector3(0, _PosY, 0);
            if (count > 15)
            {
                break;
            }
            yield return null;//한프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

    }
    //지면 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //capsuleCollider 영역 반만큼의 y값 만큼 아래방향으로 Ray가 나온다. -> 땅과 닿는다.
        theCrosshair.JumpingAnimation(!isGround);
    }

    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    //점프
    private void Jump()
    {
        //앉은 상태에서 점프 시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        theStatusController.DereaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    //달리기 시도
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

    //달리기 실행
    private void Running()
    {
        //앉은 상태에서 달리기 시 앉은 상태 해제
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
    {//플레이어 이동
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //-1, 0, 1 중 하나가 반환, 키보드 입력에 즉시 반응
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // GetAxis는 -1.0f부터 1.0f까지의 값을 반환, 부드러운 이동에 사용 

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //벡터 요소 합을 1로 맞춘다.

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        MoveCheck();
        Move();

        TryRun();

    }

    //움직임 체크
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


    //상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//값을 두 값 사이로 제한

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //좌우 플레이어 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _CharacterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_CharacterRotationY));

    }

}
