using UnityEngine;

public class HoseNozzle : Grabbable
{
    [SerializeField]
    ParticleSystem waterParticleSystem;

    public override void Interact(Player player)
    {
        Grab(player);
    }

    public override void Use(bool isUsed)
    {
        if (isUsed)
        {
            ActivateWater(true);
        }
        else
        {
            ActivateWater(false);
        }
    }

    private void ActivateWater(bool isActive)
    {
        ParticleSystem.EmissionModule emissionModule = waterParticleSystem.emission;
        emissionModule.enabled = isActive;
    }
}
