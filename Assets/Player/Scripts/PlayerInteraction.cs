using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;
    private PlayerInputs _input;
    private PlayerController _controller;

    private Interactable highlightedInteractable;
    private Grabbable heldObject;

    public float throwForce = 10f;

    void Start()
    {
        player = GetComponent<Player>();
        _input = player.Inputs;
        _controller = player.Controller;
    }

    void Update()
    {
        Use();
        Interaction();
        RotateShootable();
    }

    private void RotateShootable()
    {
        if (heldObject != null && heldObject is Shootable shoo)
        {
            Vector3 aimDirection = (_controller.mouseWorldPosition-shoo.shootPoint.position).normalized;
            shoo.shootPoint.forward = Vector3.Lerp(shoo.shootPoint.forward, aimDirection, Time.deltaTime * 20f);
        }
    }

    private void Use()
    {
       if(_input.use)
        {
            if(heldObject!=null)
            {
                heldObject.Use(true);
            }
        }
        else
        {
            if(heldObject!=null)
            {
                heldObject.Use(false);
            }
        }
    }

    private void Interaction()
    {
        if(_input.interact)
        {
            if(highlightedInteractable!=null && heldObject==null)
            {
                if (highlightedInteractable.GetComponent<Grabbable>() is Grabbable obj)
                {
                    highlightedInteractable.Interact(player);
                    Grab(obj);
                }
                else
                {
                    highlightedInteractable.Interact(player);
                }
            }
            else if(heldObject!=null)
            {
                heldObject.Use(false);
                heldObject.Throw(player.grabTransform.forward, throwForce);
                heldObject = null;
            }
            _input.interact = false;
        }
    }

    private void Grab(Grabbable obj)
    {
        heldObject = obj;
        heldObject.Highlight(false);
        highlightedInteractable = null;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (heldObject) return;
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            highlightedInteractable = interactable;
            interactable.Highlight(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (heldObject) return;
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            highlightedInteractable = null;
            interactable.Highlight(false);
        }
    }
}
