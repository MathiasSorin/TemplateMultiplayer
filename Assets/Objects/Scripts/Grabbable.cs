using UnityEngine;

public class Grabbable : Interactable
{
    public Player holder;
    [SerializeField]
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    public virtual Grabbable Grab(Player player)
    {
        holder = player;
        transform.SetParent(player.grabTransform);
        transform.position = player.grabTransform.position;
        transform.rotation = player.grabTransform.rotation;
        rb.isKinematic = true;
        return this;
    }
    public virtual void Use(bool isUsed) {}
    public virtual void Drop() {}
    public virtual void Throw(Vector3 direction, float force)
    {
        holder = null;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
