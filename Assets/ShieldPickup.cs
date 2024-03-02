using Fusion;
using UnityEngine;

public class ShieldPickup : NetworkBehaviour
{
    public float duration = 10f; // Duration of the shield

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out var health))
        {
            health.ActivateShieldRpc(duration);
            // Networked removal of the shield pickup
            if (Object.HasStateAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}
