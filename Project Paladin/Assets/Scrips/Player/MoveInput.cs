using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    CharacterController2D _controller;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _controller._moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            _controller.attack = true;
        }
        else
            _controller.attack = false;

        if (Input.GetButtonDown("Jump"))
        {
            _controller.jump = true;
        }
        else
            _controller.jump = false;
    }
}
