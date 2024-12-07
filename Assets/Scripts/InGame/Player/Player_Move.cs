using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private IEnumerator IdleRoutine()
    {
        ChangeAnimationState(0);
        HandleInput();
        if (IsMoving())
        {
            ChangeState(ePLAYERSTATE.MOVE);
        }
        yield return null;
    }

    private IEnumerator MoveRoutine()
    {
        HandleMovement();
        HandleInput();
        if (!IsMoving())
        {
            ChangeState(ePLAYERSTATE.IDLE);
        }
        //����� �߰�����

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

    private void HandleInput()//�̵� �� �Է� ó��
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)//����Ű�� �Է����� ��
        {
            ChangeState(ePLAYERSTATE.ATTACK);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))//����Ű�� �Է����� ��
        {
            HandleJump();
        }

        //if(Input.GetMouseButtonDown(1)) ////���Ű�� �Է����� ��
        //{
        //    ChangeState(ePLAYERSTATE.DEFEND);
        //}
    }

    private bool IsMoving()//�÷��̾��� �̵� üũ
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void HandleJump()
    {
        // ���� ��� ���� ���� ���� ����
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ����
        {
            Debug.Log("Jump");
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ���� ���� ����
        }
    }
}
