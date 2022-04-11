using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float ForwardSpeedMultiplier = 6f;
    public float BackwardSpeedMultiplier = 1f;
    public float StrafeSpeedMultiplier = 4f;

    private Animator _animator;
    private float _currentSpeed = 1f;
    private float _targetSpeed = 1f;
    

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // normalizing input
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
        // if no key pressed -> decrease animation controller values and jump to idle state on low values
        if (movement.magnitude == 0)
        {
            _animator.SetFloat("Horizontal", movement.x, 0.2f, Time.deltaTime);
            _animator.SetFloat("Vertical", movement.z, 0.2f, Time.deltaTime);
            if (Math.Abs(_animator.GetFloat("Horizontal")) < 0.1 && Math.Abs(_animator.GetFloat("Vertical")) < 0.1)
            {
                _targetSpeed = 0f;
                _animator.SetBool("IsIdle", true);
            }
        }
        else _animator.SetBool("IsIdle", false);
        
        if (_animator.GetBool("IsIdle")) return;
        
        // specifying _targetSpeed by identifying pressed direction key
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                _targetSpeed = (ForwardSpeedMultiplier + StrafeSpeedMultiplier) / 2;
            }
            else _targetSpeed = ForwardSpeedMultiplier;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                _targetSpeed = (BackwardSpeedMultiplier + StrafeSpeedMultiplier) / 2;
            }
            else _targetSpeed = BackwardSpeedMultiplier;
        }
        else
        {
            _targetSpeed = StrafeSpeedMultiplier;
        }
        
        // prevents from sudden direction change in vertical axis (AD-AD strafing doesn't affected)
        if (_targetSpeed < _currentSpeed)
        {
            _currentSpeed = 0;
        }
        _currentSpeed = Mathf.SmoothStep(_currentSpeed, _targetSpeed, 0.05f);
        
        // character moving
        transform.position += movement * _currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.identity;
        Debug.Log("speed: " + _currentSpeed);
        
        // applying animation values
        _animator.SetFloat("Horizontal", movement.x, 0.4f, Time.deltaTime);
        _animator.SetFloat("Vertical", movement.z, 0.4f, Time.deltaTime);
    }
}
