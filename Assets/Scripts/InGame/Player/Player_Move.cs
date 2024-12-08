using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private IEnumerator IdleRoutine()
    {
        ChangeAnimationState(0);
        yield return null;
    }

    private IEnumerator MoveRoutine()
    {
        HandleMovement();
        yield return null;
    }

    private void HandleMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        bool ShiftRun = Input.GetButton("LeftShift");

        direction = new Vector3(Horizontal, 0, Vertical).normalized * moveSpeed;

        if (direction != Vector3.zero)
        {
            ChangeAnimationState(1);
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(Horizontal, Vertical) * Mathf.Rad2Deg, 0);
            rigidBody.MovePosition(rigidBody.position + direction * Time.deltaTime);
        }

        if (ShiftRun)
        {
            ChangeAnimationState(6);
            direction *= 0.3f;
        }
    }


    private bool IsMoving()//플레이어의 이동 체크
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void HandleJump()
    {
        // 땅에 닿아 있을 때만 점프 가능
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프 힘을 적용
        }
    }
}
