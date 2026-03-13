using UnityEngine;

public class StaticLiquidSource : Interactable
{
    void OnTriggerStay(Collider other)
    {
        GrabbableLiquidSource grabbableLiquidSource = other.GetComponent<GrabbableLiquidSource>();
        if (grabbableLiquidSource != null)
        {
            grabbableLiquidSource.Fill();
        }
    }
}
