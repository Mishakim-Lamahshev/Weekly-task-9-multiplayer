using Fusion;
using UnityEngine;
using System.Collections;

public class Health : NetworkBehaviour
{
    [SerializeField] private NumberField HealthDisplay;

    [Networked(OnChanged = nameof(OnNetworkedHealthChanged))]
    public int NetworkedHealth { get; set; } = 100;

    [Networked] private bool isShieldActive { get; set; } = false; // Track shield state

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    private static void OnNetworkedHealthChanged(Changed<Health> changed)
    {
        changed.Behaviour.HealthDisplay.SetNumber(changed.Behaviour.NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        if (isShieldActive) return; // Ignore damage if shield is active

        NetworkedHealth -= damage;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ActivateShieldRpc(float duration)
    {
        StartCoroutine(ShieldDuration(duration));
    }

    private IEnumerator ShieldDuration(float duration)
    {
        isShieldActive = true;
        yield return new WaitForSeconds(duration);
        isShieldActive = false;
    }
}
