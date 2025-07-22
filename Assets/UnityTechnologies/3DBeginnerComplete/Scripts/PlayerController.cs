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
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // y값은 중력유지

        // 회전
        if (movement.sqrMagnitude > 0.0001f)
        {
            targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation,
                                Time.fixedDeltaTime * rotationSpeed);
        }

        float angleDiff = Quaternion.Angle(rb.rotation, targetRotation);
        if (angleDiff > 0.5f) // 회전각의 차이가 0.5도 까지는 계속 회전하고
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
        else // 0.5도 이내로 근접했으면 목표회전값으로 완료
        {
            rb.rotation = targetRotation;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
