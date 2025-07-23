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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        movementInput = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        // �̵�
        Vector3 movement = movementInput * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // ȸ�� ó��
        if (movement.sqrMagnitude > 0.0001f)
        {
            targetRotation = Quaternion.LookRotation(movement);
            Quaternion qt = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);

            // ȸ���� ���� �Ϸ�(0.5�� �̳���)�Ǿ����� ��Ȯ�� ������
            if (Quaternion.Angle(rb.rotation, targetRotation) <= 0.5f)
            {
                rb.rotation = targetRotation;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                rb.rotation = Quaternion.Normalize(qt); // �����ϰ� ����ȭ
            }
        }
    }
}
