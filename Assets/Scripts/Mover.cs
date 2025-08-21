using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I'm a new Script");
    }

    // Update is called once per frame
    void Update()
    {
        //Get the horizontal and vertical input values
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //create a Vector3 to store the movement direction
        Vector3 movement = new Vector3(horizontalInput,0,verticalInput);

        //Move the object based on input,speed, and frame rate

        transform.Translate(movement*speed*Time.deltaTime);
    }
}
