using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float force;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float smoothTime;
    float smoothVelocity;
    public Transform firstCamera;

    // Переменные для прыжка
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;

    private bool isJumping;
    private bool isHitting;

    // Переменные для звука бега
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip runSound;

    // Переменная для отслеживания состояния курсора
    private bool isCursorVisible = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Проверка на нажатие клавиши Tab для показа/скрытия курсора
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isCursorVisible = !isCursorVisible;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + firstCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
            animator.SetBool("isRunning", true);
            characterController.Move(move.normalized * force * Time.deltaTime);

            // Воспроизведение звука бега
            if (!audioSource.isPlaying)
            {
                audioSource.clip = runSound;
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            audioSource.Stop(); // Остановка звука бега, когда персонаж не бежит
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("isHitting", true);
        }
        else
        {
            animator.SetBool("isHitting", false);
        }

        // Прыжок
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

        // Применение гравитации
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isJumping = false;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Сброс параметра прыжка после завершения прыжка
        if (characterController.isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }
}
