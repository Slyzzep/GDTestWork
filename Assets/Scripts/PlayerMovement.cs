using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private int _moveSpeed;
    void Start()
    {
        
    }

  
    void Update()
    {
        InputButtons();
        PlayerMove();
    }
    public void InputButtons()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }
    public void PlayerMove()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _verticalInput + right * _horizontalInput;
        //transform.Translate(new Vector3(_horizontalInput, 0f, _verticalInput) * _moveSpeed * Time.deltaTime);
        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
    }
}
