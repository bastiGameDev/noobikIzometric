using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // �������� ������������
    [SerializeField] private float rotationSpeed = 10f; // �������� ��������
    [SerializeField] private Animator animator; // ������ �� Animator

    private CharacterController characterController;

    private void Start()
    {
        // �������� ��������� CharacterController
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �������� ���� �� ������
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // ������ ����������� ��������
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // ���������� �������� ����
            animator.SetBool("isRunning", true);

            // ������������ ��������� � ����������� ��������
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ���������� ���������
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            // ��������� �������� ����, ���� ��� ��������
            animator.SetBool("isRunning", false);
        }
    }
}
