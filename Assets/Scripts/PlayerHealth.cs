using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode; // Make sure this is here!

public class PlayerHealth : NetworkBehaviour
{
    // Our health is now a NetworkVariable, which syncs from server to clients.
    // The <float> is a generic type, so you can use it for any type of variable.
    public NetworkVariable<float> Health = new NetworkVariable<float>(100f);

    // This is a reference to the UI Slider we created in a previous lesson
    [SerializeField] private Slider healthSlider;

    public override void OnNetworkSpawn()
    {
        // Subscribe to a callback that runs when the NetworkVariable changes
        Health.OnValueChanged += OnHealthChanged;

        // Set the initial value on all clients
        healthSlider.maxValue = Health.Value;
        healthSlider.value = Health.Value;
    }

    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        healthSlider.value = newHealth;

        if (newHealth <= 0)
        {
            Die();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float amount)
    {
        // Only the server is allowed to change the NetworkVariable.
        // We add an if check to be safe, but the ServerRpc attribute helps enforce this.
        if (IsServer)
        {
            Health.Value -= amount;
        }
    }

    private void Die()
    {
        Debug.Log("Player has been defeated!");
        // We will add more game over logic here later
    }
}