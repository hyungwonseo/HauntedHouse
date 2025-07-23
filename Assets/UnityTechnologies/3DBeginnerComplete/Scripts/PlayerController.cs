using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f; // 이동속도
    public float rotationSpeed = 10f; // 회전속도
    Rigidbody rb; 
    Animator animator;
    Vector3 movementInput; // 방향벡터    
    Quaternion targetRotation;
    Quaternion qt;
    bool isDancing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDancing)
            return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        // 월드좌표 기준으로 방향이 고정되어있음 (예, 방향윗키는 월드좌표의 북쪽으로 고정)
        // movementInput = new Vector3(moveX, 0, moveZ).normalized;

        // 카메라 기준으로 방향을 설정함
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        // y(높이)축을 회전 계산에서 빼야 함
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        movementInput = (cameraForward * moveZ + cameraRight * moveX).normalized;

        // 댄스 애니메이션 키 입력
        if (Input.GetKeyDown(KeyCode.Space) && !isDancing)
        {
            StartCoroutine(PlayDanceAnimation());
        }

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (isDancing)
            return;

        // 이동
        Vector3 movement = movementInput * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // y값은 중력유지        

        // 회전
        if (movement.sqrMagnitude > 0.01f)
        {
            targetRotation = Quaternion.LookRotation(movement);
            qt = Quaternion.Slerp(rb.rotation, targetRotation,
                                Time.fixedDeltaTime * rotationSpeed);
            rb.rotation = Quaternion.Normalize(qt);
        }
        else
        {
            float angleDiff = Quaternion.Angle(rb.rotation, targetRotation);
            if (angleDiff > 0.5f) // 회전각의 차이가 0.5도 까지는 계속 회전하고
            {
                rb.rotation = Quaternion.Normalize(qt);
            }
            else // 0.5도 이내로 근접했으면 목표회전값으로 완료
            {
                rb.rotation = targetRotation;
                rb.angularVelocity = Vector3.zero;
            }
        }        
    }

    void UpdateAnimation()
    {
        if (movementInput.sqrMagnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator PlayDanceAnimation()
    {
        isDancing = true;
        animator.SetTrigger("Dance");
        animator.SetBool("isDancing", true);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
        {
            yield return null; // 다음 프레임에 다시 확인해보자
        }

        float duration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        isDancing = false;
        animator.SetBool("isDancing", false);
    }
}
