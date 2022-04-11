using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    public float Forward = 1f;
    public float Backward = 1f;
    public float Strafe = 1f;

    private Animator animator;
    private float CurrentSpeed = 1f;
    private float TargetSpeed = 1f;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (movement.magnitude == 0)
        {
            animator.SetFloat("Horizontal", movement.x, 0.2f, Time.deltaTime);
            animator.SetFloat("Vertical", movement.z, 0.2f, Time.deltaTime);
            if (Math.Abs(animator.GetFloat("Horizontal")) < 0.1 && Math.Abs(animator.GetFloat("Vertical")) < 0.1)
            {
                TargetSpeed = 0f;
                animator.SetBool("IsIdle", true);
            }
        }
        else
        {
            animator.SetBool("IsIdle", false);
        }

        bool pressedW = Input.GetAxisRaw("Vertical") > 0;
        bool pressedA = Input.GetAxisRaw("Horizontal") < 0;
        bool pressedS = Input.GetAxisRaw("Vertical") < 0;
        bool pressedD = Input.GetAxisRaw("Horizontal") > 0;
        
        if (!animator.GetBool("IsIdle"))
        {
            if (pressedW)
            {
                if (pressedA || pressedD)
                {
                    TargetSpeed = (Forward + Strafe) / 2;
                }
                else TargetSpeed = Forward;
            }
            else if (pressedS)
            {
                if (pressedA || pressedD)
                {
                    TargetSpeed = (Backward + Strafe) / 2;
                }
                else TargetSpeed = Backward;
            }
            else
            {
                TargetSpeed = Strafe;
            }

            if (TargetSpeed < CurrentSpeed)
            {
                CurrentSpeed = 0;
            }

            CurrentSpeed = Mathf.SmoothStep(CurrentSpeed, TargetSpeed, 0.05f);
            transform.position += movement * CurrentSpeed * Time.deltaTime;
            //var targetPosition = transform.position + movement * Speed;
            //transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
            transform.rotation = Quaternion.identity;
            
            Debug.Log("speed: " + CurrentSpeed);
            
            animator.SetFloat("Horizontal", movement.x, 0.4f, Time.deltaTime);
            animator.SetFloat("Vertical", movement.z, 0.4f, Time.deltaTime);
        }
    }
}
