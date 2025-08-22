using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    public Transform cameraHolder;
    private float xRotation = 0f;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //This locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse-Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation,0f,0f);
        transform.Rotate(Vector3.up*mouseX);

        //keyboard movement

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move*speed*Time.deltaTime);

        //shooting logic
        if (Input.GetMouseButtonDown(0))//Left mouse button
        {
            Shoot();
        }

    }
    void Shoot()
    {
        muzzleFlash.Play(); // Play the particle system
        RaycastHit hit;
        if (Physics.Raycast(cameraHolder.position,cameraHolder.forward, out hit))
        {
            //log what we hit to console
            Debug.Log(hit.collider.name);
            // create the impact efect at the exact hit point
            GameObject impactGO = Instantiate(impactEffect,hit.point,Quaternion.LookRotation(hit.normal));
            // destroy the impact after a short time
            Destroy(impactGO,2f);

            //if the object we hit has the tag 'Target'

            if (hit.collider.CompareTag("Target"))
            {
                // Destroy(hit.collider.gameObject);//Destroy the object
                Target target = hit.collider.GetComponent<Target>();
                if (target !=null)
                {
                    target.TakeDamage(damage);
                }

            }
        }
    }
}
