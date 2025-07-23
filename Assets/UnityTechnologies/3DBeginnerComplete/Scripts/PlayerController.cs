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
        // 이동
        Vector3 movement = movementInput * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // 회전 처리
        if (movement.sqrMagnitude > 0.0001f)
        {
            targetRotation = Quaternion.LookRotation(movement);
            Quaternion qt = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);

            // 회전이 거의 완료(0.5도 이내로)되었으면 정확히 맞춰줌
            if (Quaternion.Angle(rb.rotation, targetRotation) <= 0.5f)
            {
                rb.rotation = targetRotation;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                rb.rotation = Quaternion.Normalize(qt); // 안전하게 정규화
            }
        }
    }
}
