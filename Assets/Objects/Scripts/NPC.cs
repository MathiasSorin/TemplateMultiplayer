using UnityEngine;

public class NPC : Talkable
{
    public override void Interact(Player player)
    {
        Talk();
    }
}
