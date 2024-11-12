using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public float _horizontalInput{ get; private set; }
    public float _verticalInput { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void InputButtons()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
    }
    private void FixedUpdate()
    {
        InputButtons();
    }


}
