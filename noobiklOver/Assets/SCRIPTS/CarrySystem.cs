using UnityEngine;

public class CarrySystem : MonoBehaviour
{
    [SerializeField] private Transform carryPoint; // Точка, куда будет привязан предмет
    [SerializeField] private float pickUpRange = 2f; // Максимальная дистанция для подбора предмета
    [SerializeField] private LayerMask pickableLayer; // Слой для подбора предметов

    private GameObject carriedObject; // Текущий переносимый предмет
    [SerializeField]  private Animator animator;

    private void Update()
    {
        // Проверяем нажатие левой кнопки мыши для подбора предмета
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            TryPickUp();
        }

        // Проверяем нажатие клавиши E для сброса предмета
        if (Input.GetKeyDown(KeyCode.E) && carriedObject != null)
        {
            DropObject();
        }
    }

    private void TryPickUp()
    {
        // Получаем позицию курсора в мировых координатах
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, pickableLayer))
        {
            // Проверяем, является ли объект "подбираемым"
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Pickable"))
            {
                // Проверяем расстояние до объекта
                float distance = Vector3.Distance(transform.position, hitObject.transform.position);
                if (distance <= pickUpRange)
                {
                    PickUpObject(hitObject);
                }
            }
        }
    }

    private void PickUpObject(GameObject objectToPickUp)
    {
        // Отключаем физику у предмета
        Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Привязываем предмет к точке переноски
        objectToPickUp.transform.SetParent(carryPoint);
        objectToPickUp.transform.localPosition = Vector3.zero;
        objectToPickUp.transform.localRotation = Quaternion.identity;

        // Устанавливаем состояние "переноски" в Animator
        animator.SetBool("isCarry", true);

        // Сохраняем ссылку на переносимый предмет
        carriedObject = objectToPickUp;
    }

    private void DropObject()
    {
        if (carriedObject == null) return;

        // Возвращаем физику предмету
        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Отвязываем предмет от точки переноски
        carriedObject.transform.SetParent(null);

        // Размещаем предмет перед персонажем
        carriedObject.transform.position = transform.position + transform.forward * 1.5f;

        // Сбрасываем состояние "переноски" в Animator
        animator.SetBool("isCarry", false);

        // Очищаем ссылку на переносимый предмет
        carriedObject = null;
    }
}