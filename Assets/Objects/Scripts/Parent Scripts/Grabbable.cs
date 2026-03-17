using System;
using UnityEngine;

public class Grabbable : Interactable
{
    public Type canBeUsedOnType;

    private Player holder;
    private Rigidbody rb;
    private Collider col;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public virtual Grabbable Grab(Player player)
    {
        holder = player;
        transform.SetParent(player.grabTransform);
        transform.position = player.grabTransform.position;
        transform.rotation = player.grabTransform.rotation;
        EnablePhysics(false);
        return this;
    }

    public virtual void Use(bool isUsed, Interactable target = null) {}

    public virtual void Drop()
    {
        holder = null;
        transform.SetParent(null);
        EnablePhysics(false);
    }

    public virtual void Throw(Vector3 direction, float force)
    {
        holder = null;
        transform.SetParent(null);
        EnablePhysics(true);
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    protected void EnablePhysics(bool on)
    {
        rb.isKinematic = !on;
        col.isTrigger = !on;
    }
}
