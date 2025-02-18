using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D playerRB;
    private float moveHorizontal; // left and right
    private float moveVertical; // up and down

    // Integer: 1, 2, 67, 100
    // Float: 1.1, 6.7, 0.38, 4.0

    private Vector2 movement;

    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        movement = new Vector2(moveHorizontal, moveVertical);

        playerRB.velocity = movement * speed;

    }
}
