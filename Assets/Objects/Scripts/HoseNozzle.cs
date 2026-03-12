using NUnit.Framework;
using UnityEngine;

public class HoseNozzle : Shootable
{
    public float waterUsagePerSecond = -0.5f;

    [SerializeField]
    private Hose hose;
    [SerializeField]
    ParticleSystem waterParticleSystem;

    public override void Interact(Player player)
    {
        Grab(player);
    }

    public override void Use(bool isUsed, Interactable target = null)
    {
        if (isUsed)
        {
            if(hose.connector.connectedWaterSource != null)
            {
                ShootWater(hose.connector.connectedWaterSource.UpdateSourceAmount(-waterUsagePerSecond * Time.deltaTime));
            }
        }
        else
        {
            ShootWater(false);
        }
    }

    private void ShootWater(bool isActive)
    {
        ParticleSystem.EmissionModule emissionModule = waterParticleSystem.emission;
        emissionModule.enabled = isActive;
    }
}
