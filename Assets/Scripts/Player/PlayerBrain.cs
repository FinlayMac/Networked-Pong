using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    private PlayerControls controls;
    public float PlayerSpeed = 10f;

    void Awake()
    {
        controls = new PlayerControls();
        //controls.Paddle.Activate.performed += ctx => SpawnBall();
        controls.Paddle.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Paddle.Move.canceled += ctx => Move(Vector2.zero);
    }

    Vector2 playerDirection = Vector2.zero;
    void Move(Vector2 newDirection)
    {
        playerDirection = newDirection;
        Debug.Log("player moved " + newDirection);
    }

    private void Update()
    { transform.position += Vector3.up * playerDirection.y * Time.deltaTime * PlayerSpeed; }


    private void OnEnable() { controls.Paddle.Enable(); }

    private void OnDisable() { controls.Paddle.Disable(); }
}
