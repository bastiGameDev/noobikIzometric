using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Скорость передвижения
    [SerializeField] private float rotationSpeed = 10f; // Скорость поворота
    [SerializeField] private Animator animator; // Ссылка на Animator

    private CharacterController characterController;

    private void Start()
    {
        // Получаем компонент CharacterController
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Получаем ввод от игрока
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Создаём направление движения
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Активируем анимацию бега
            animator.SetBool("isRunning", true);

            // Поворачиваем персонажа в направлении движения
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Перемещаем персонажа
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Отключаем анимацию бега, если нет движения
            animator.SetBool("isRunning", false);
        }
    }
}
