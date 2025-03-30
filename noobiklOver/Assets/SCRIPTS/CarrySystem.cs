using UnityEngine;

public class CarrySystem : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private float pickUpRange = 2f;
    public GameObject carriedObject;
    [SerializeField] private Animator animator;

    public GameObject GetCarriedObject()
    {
        return carriedObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickUp();
        }

        if (Input.GetKeyDown(KeyCode.E) && carriedObject != null)
        {
            DropObject();
        }
    }

    private void TryPickUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ”бираем использование LayerMask и провер€ем только теги
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Pickable"))
            {
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
        Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        objectToPickUp.transform.SetParent(carryPoint);
        objectToPickUp.transform.localPosition = Vector3.zero;
        objectToPickUp.transform.localRotation = Quaternion.identity;

        animator.SetBool("isCarry", true);
        carriedObject = objectToPickUp;
    }

    public void ClearCarryAndTeleportTo(Vector3 vector3)
    {
        if (carriedObject == null) return;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        carriedObject.transform.SetParent(null);
        carriedObject.transform.position = vector3;

        animator.SetBool("isCarry", false);
        carriedObject = null;
    }

    public void DropObject()
    {
        if (carriedObject == null) return;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        carriedObject.transform.SetParent(null);
        carriedObject.transform.position = transform.position + transform.forward * 1.5f;

        animator.SetBool("isCarry", false);
        carriedObject = null;
    }
}