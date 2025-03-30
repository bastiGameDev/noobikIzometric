using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCarry : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isCarry", true);

        Debug.Log("triggered");
        
    }
}
