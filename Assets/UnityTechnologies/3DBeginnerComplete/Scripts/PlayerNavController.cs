using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavController : MonoBehaviour
{    
    public float rotationSpeed = 10f; // 회전속도
    Rigidbody rb; 
    Animator animator;
    
    Quaternion targetRotation;
    Quaternion qt;
    bool isDancing = false;

    NavMeshAgent agent; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX |
                            RigidbodyConstraints.FreezePositionZ |
                            RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationZ;
        }

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDancing)
            return;

        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 클릭
        {
            // agent 이동 명령
            MoveToClickPoint();
        }

        // 댄스 애니메이션 키 입력
        if (Input.GetKeyDown(KeyCode.Space) && !isDancing)
        {
            StartCoroutine(PlayDanceAnimation());
        }

        UpdateAnimation();
    }

    // Navigation 시스템을 사용시 Rigidbody는 원래 필요없음
    // 하지만 회전의 용도로만 사용하는 것은 권장되는 방법임
    // 물리적인 특징과 이동의 목적으로는 사용하지 않으므로 관련 속성을 막을 수 있음
    // isKinematic, FreezePosition, FreezeRotation 등
    // 만약 충돌, 밀림, 낙하등의 효과가 필요한 경우에는 Rigidbody를 활성화해서 사용해야함
    void FixedUpdate()
    {
        if (isDancing)
            return;                       

        // 회전
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            targetRotation = Quaternion.LookRotation(agent.velocity);
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
        if (agent.velocity.sqrMagnitude > 0)
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

    void MoveToClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);
        }
    }
}
