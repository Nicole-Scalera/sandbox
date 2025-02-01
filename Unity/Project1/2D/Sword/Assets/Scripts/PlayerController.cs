using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // RigidBody component instance for the player
    private Rigidbody2D playerRB;

    // Track how much movement is needed from the player's input
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 movement;

    // Speed modifier for player movement
    public float speed = 4;

    // Initialize any component references
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

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
