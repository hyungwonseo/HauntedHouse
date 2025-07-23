using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavController : MonoBehaviour
{    
    public float rotationSpeed = 10f; // ȸ���ӵ�
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

        if (Input.GetMouseButtonDown(0)) // ���� ���콺 Ŭ��
        {
            // agent �̵� ���
            MoveToClickPoint();
        }

        // �� �ִϸ��̼� Ű �Է�
        if (Input.GetKeyDown(KeyCode.Space) && !isDancing)
        {
            StartCoroutine(PlayDanceAnimation());
        }

        UpdateAnimation();
    }

    // Navigation �ý����� ���� Rigidbody�� ���� �ʿ����
    // ������ ȸ���� �뵵�θ� ����ϴ� ���� ����Ǵ� �����
    // �������� Ư¡�� �̵��� �������δ� ������� �����Ƿ� ���� �Ӽ��� ���� �� ����
    // isKinematic, FreezePosition, FreezeRotation ��
    // ���� �浹, �и�, ���ϵ��� ȿ���� �ʿ��� ��쿡�� Rigidbody�� Ȱ��ȭ�ؼ� ����ؾ���
    void FixedUpdate()
    {
        if (isDancing)
            return;                       

        // ȸ��
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
            if (angleDiff > 0.5f) // ȸ������ ���̰� 0.5�� ������ ��� ȸ���ϰ�
            {
                rb.rotation = Quaternion.Normalize(qt);
            }
            else // 0.5�� �̳��� ���������� ��ǥȸ�������� �Ϸ�
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
            yield return null; // ���� �����ӿ� �ٽ� Ȯ���غ���
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
