using UnityEngine;

public class Trash : Grabbable
{
    public override void Interact(Player player)
    {
        Grab(player);
    }
}
