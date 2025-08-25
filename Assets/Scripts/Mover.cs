using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
  //  [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float horizontalSpeed = 300f;
    private Rigidbody rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I'm a new Script");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the horizontal and vertical input values
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //create a Vector3 to store the movement direction
        Vector3 movement = new Vector3(horizontalInput,0,verticalInput);

        //Move the object based on input,speed, and frame rate

        // transform.Translate(movement*speed*Time.deltaTime);
       // Debug.Log("Horizontal Input: "+horizontalInput + " | Vertical Input: "+verticalInput);

        rb.AddForce(movement*horizontalSpeed*Time.fixedDeltaTime);

        bool isGrounded = Physics.Raycast(transform.position,Vector3.down
            ,0.6f,groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }
        Debug.Log("Speed : "+ movement.magnitude);
        animator.SetFloat("Speed",movement.magnitude);

    }
}
