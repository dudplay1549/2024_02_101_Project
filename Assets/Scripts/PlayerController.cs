using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f; //이동 속도
    public float jumpForce = 5.0f; //점프 힘
    public float rotationSpeed = 10f; //회전속도

    //카메라 설정 변수
    [Header("Camera Settings")]
    public Camera firstPersonCamera; //1인칭 카메라
    public Camera thirdPersonCamera; //3인칭 카메라

    public float radius = 5.0f; //3인칭 카메라와 플레이어 간의 거리
    public float minRadius = 1.0f; //카메라 최소 거리
    public float maxRadius = 10.0f; //카메라 최대 거리

    public float yMinLimit = 30; //카메라 수직 회전 최소각
    public float yMaxLimit = 90; //카메라 수직 회전 최대각

    public float theta = 0.0f; //카메라의 수평 회전 각도
    public float phi = 0.0f; //카메라의 수직 회전 각도
    public float targetVerticalRoatation = 0; //목표 수직 회전 각도
    public float verticalRoatationSpeed = 240f; //수직 회전 속도

    public float mouseSenesitivity = 2f; //마우스 감도

    //내부 변수들
    public bool isFirstPerson = true; //1인칭 모드인지 여부
    //private bool isGrounded; //플레이어가 땅에 있는지의 여부
    private Rigidbody rb; //플레이어의 RigidBody

    public float fallingThreshold = -0.1f;  //떨어지는것으로 간주할 수직 속도 임계값

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;     //등반 가능한 최대 경사 각도
    public const int groundCheckPoints = 5;    //지면 체크 포인트 수

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //RigidBody 컴포넌트를 가져온다.

        Cursor.lockState = CursorLockMode.Locked; //마우스 커서를 잠그고 숨긴다
        SetupCameras();
        SetActiveCamera();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleCameraToggle();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void SetActiveCamera() 
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson); //1인칭 카메라 활성화 여부
        thirdPersonCamera.gameObject.SetActive(isFirstPerson); //3인칭 "
    }
    //카메라 및 캐릭터 회전 처리하는 함수
    public void HandleRotation()  
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity; //마우스 좌우 입력
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity; //마우스 상하 입력

        //수평 회선 (theta 값)
        theta += mouseX; //마우스 입력값 추가
        theta = Mathf.Repeat(theta, 360f); //각도 값이 360 넘지 않도록 조성

        //수직 회전 처리
        targetVerticalRoatation -= mouseY;
        targetVerticalRoatation = Mathf.Clamp(targetVerticalRoatation, yMinLimit, yMaxLimit); //수직 회전 제한
        phi = Mathf.MoveTowards(phi, targetVerticalRoatation, verticalRoatationSpeed * Time.deltaTime);

        if (isFirstPerson) 
        {
            //플레이어 회전(캐릭터가 수평으로만 회전)
            transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(0.0f, theta, 0.0f);
        }
        else
        {
            //3인칭 카메라 구면 좌표계에서 위치 및 회전 계산
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * phi);
            float z = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * theta);

            thirdPersonCamera.transform.position = transform.position + new Vector3(x, y, z);
            thirdPersonCamera.transform.LookAt(transform); //카메라가 항상 플레이어를 바라보도록 설정

            //마우스 스크롤을 사용하여 카메라 줌 조성
            radius = Mathf.Clamp(radius - Input.GetAxis("Mouse ScrollWheel") * 5, minRadius, maxRadius);
        }
        firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f); //1인칭 카메라 수직 회전
    }

    //1인칭과 3인칭 카메라를 전환하는 함수
    public void HandleCameraToggle() 
    {
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            isFirstPerson = !isFirstPerson; //카메라 모드 전환
            SetActiveCamera();
        }
    }
    //카메라 초기 위치 및 회전을 설정하는 함수
    void SetupCameras() 
    {
        firstPersonCamera.transform.localPosition = new Vector3(0f, 0.6f, 0f); //1인칭 카메라 위치
        firstPersonCamera.transform.localRotation = Quaternion.identity; //1인칭 카메라 회전 초기화
    }
    //플레이어 점프를 처리하는 함수  
    void HandleJump()
    {
        //점프 버튼을 누르고 땅에 있을 때
        if(isGrounded()) 
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //위쩍으러 힘을 가해 점프
        }
    }
    
    //플레이어의 이동을 처리하는 함수
    public void HandleMovement() 
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); //좌우 입력 (-1,1)
        float moveVertical = Input.GetAxis("Vertical"); //앞뒤 입력 (1, -1)

        Vector3 movement;
        if(!isFirstPerson) //3인칭 모드 일 때, 카메라 방향으로 이동 처리
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward; //카메라 앞 방향
            cameraForward.y = 0f; //수직 방향 제거
            cameraForward.Normalize(); //방향 벡터 정규화 (0~1) 사으의 값으로 만들어준다

            Vector3 cameraRight = thirdPersonCamera.transform.right; //카메라 오른쪽 방향
            cameraRight.y = 0f;
            cameraRight.Normalize();

            //이동 벡터 계산
            movement = cameraForward * moveVertical + cameraRight * moveHorizontal;
        }
        else 
        {
            //캐릭터 기준으로 이동(1인칭)
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        }

        //이동 방향으로 캐릭터 회전
        if (movement.magnitude > 0.1f) 
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

    }    

    public bool isFalling()     //떨어지는 유무 확인
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()    //땅 체크 확인
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public float GetVerticalVelocity() //플레이어의 Y축 속도 확인
    {
        return rb.velocity.y;
    }
}