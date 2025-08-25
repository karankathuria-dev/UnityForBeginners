using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class MultiplayerPlayer : NetworkBehaviour
{
    // A configurable speed for our player
    [SerializeField] private float speed = 5f;

    // A reference to the camera for raycasting
    [SerializeField] private Camera playerCamera;

    // The visual effect for the impact, assigned in the Inspector
    [SerializeField] private GameObject impactEffectPrefab;

    void Awake()
    {
        // Get the camera reference
        playerCamera = GetComponentInChildren<Camera>();
    }

    // This function runs on every frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        // Handle movement input (already in your script)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(movement * speed * Time.deltaTime);

        // Check for shooting input
        if (Input.GetMouseButtonDown(0))
        {
            // The client tells the server it wants to shoot
            ShootServerRpc();
        }
    }

    // This function is called by a client, but runs on the server.
    // The [ServerRpc] attribute is mandatory.
    [ServerRpc]
    private void ShootServerRpc()
    {
        // The server performs the raycast to be the source of truth
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
        {
            // Try to get the PlayerHealth component from the object we hit
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // The server calls the TakeDamageServerRpc on the target player's object.
                // This ServerRpc will then reduce the health of the target player.
                playerHealth.TakeDamageServerRpc(10f);
            }

            // Now, tell all clients to show the impact effect at the hit location
            // We pass the position and rotation as parameters to the ClientRpc
            DisplayImpactClientRpc(hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    // This function is called by the server, but runs on all clients.
    // The [ClientRpc] attribute is mandatory.
    [ClientRpc]
    private void DisplayImpactClientRpc(Vector3 position, Quaternion rotation)
    {
        if (!NetworkManager.IsClient) return;

        // Get an object from the pool instead of instantiating it
        GameObject pooledImpact = NetworkObjectPool.Singleton.GetObject();

        if (pooledImpact != null)
        {
            // Now use the pooled object
            pooledImpact.transform.position = position;
            pooledImpact.transform.rotation = rotation;

            // This is a simple way to return the object to the pool after a delay
            StartCoroutine(ReturnToPoolAfterDelay(pooledImpact, 2f));
        }
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        NetworkObjectPool.Singleton.ReturnObject(obj);
    }
}