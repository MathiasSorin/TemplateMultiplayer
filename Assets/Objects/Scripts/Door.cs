using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    private Transform transformTarget;

    public override void Interact(Player player)
    {
        player.TeleportCharacter(transformTarget);
    }
}
