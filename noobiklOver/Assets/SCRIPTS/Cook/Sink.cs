using UnityEngine;
using System.Collections;

public class Sink : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float washTime = 8f;
    [SerializeField] private Animator animator;

    [SerializeField] private Vector3 teleportTo = new Vector3(-29.2220001f, 5.90999985f, -36.3129997f);
    private CarrySystem carrySystem;
    private GameObject carriedObject;
    private bool isWashing = false;

    private void Start()
    {
        carrySystem = FindObjectOfType<CarrySystem>();
        if (carrySystem == null)
        {
            Debug.LogError("CarrySystem �� ������ � �����!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && carriedObject != null && !isWashing)
        {
            StartCoroutine(WashObject());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (carrySystem != null && carrySystem.GetCarriedObject() != null)
        {
            carriedObject = carrySystem.GetCarriedObject();
        }
        else
        {
            Debug.LogError("�� ������� �������� carriedObject!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // carriedObject = null;
    }

    private IEnumerator WashObject()
    {
        if (carriedObject == null || carrySystem == null)
        {
            Debug.LogError("carriedObject ��� CarrySystem ����� null!");
            yield break;
        }

        isWashing = true;

        // ��������� Renderer ��� ������ � ���
        Renderer objectRenderer = carriedObject.GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer �� ������ � �������!");
            yield break;
        }

        objectRenderer.enabled = false; // �������� ������

        animator.SetBool("isCarry", false);

        float timer = 0f;
        
        while (timer < washTime)
        {
            // ���������, ��� ������ �� ��� ����������
            if (carriedObject == null)
            {
                Debug.LogError("carriedObject ��� ��������� �� ����� �����!");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (dropPoint == null)
        {
            Debug.LogError("Drop Point �� ��������!");
            yield break;
        }

        // ���������� ������ �� ����� ����������
        carriedObject.transform.position = dropPoint.position;

        if (objectRenderer != null)
        {
            objectRenderer.enabled = true; // ���������� ������
        }

        // �������� DropObject ��� ������ ��������� ���������
        carrySystem.ClearCarryAndTeleportTo(teleportTo);//---------------------------------------------------------------------------------------------

        // ������� ������ �� ������
        carriedObject = null;
        isWashing = false;

        Debug.Log("����� ���������!");
    }
}