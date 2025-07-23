using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f; // �̵��ӵ�
    public float rotationSpeed = 10f; // ȸ���ӵ�
    Rigidbody rb; 
    Animator animator;
    Vector3 movementInput; // ���⺤��    
    Quaternion targetRotation;
    Quaternion qt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        // ������ǥ �������� ������ �����Ǿ����� (��, ������Ű�� ������ǥ�� �������� ����)
        // movementInput = new Vector3(moveX, 0, moveZ).normalized;

        // ī�޶� �������� ������ ������
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.Normalize();
        cameraRight.Normalize();

        movementInput = (cameraForward * moveZ + cameraRight * moveX).normalized;

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // �̵�
        Vector3 movement = movementInput * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // y���� �߷�����        

        // ȸ��
        if (movement.sqrMagnitude > 0.0001f)
        {
            targetRotation = Quaternion.LookRotation(movement);
            qt = Quaternion.Slerp(rb.rotation, targetRotation,
                                Time.fixedDeltaTime * rotationSpeed);
            rb.rotation = Quaternion.Normalize(qt);
        }

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
}
