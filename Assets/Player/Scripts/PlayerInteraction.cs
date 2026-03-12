using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;
    private PlayerInputs _inputs;
    private PlayerController _controller;

    private Interactable highlightedInteractable;
    private Grabbable heldObject;

    public float throwForce = 5f;

    void Start()
    {
        player = GetComponent<Player>();
        _inputs = player.Inputs;
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
            //TODO make this rotation number a variable
            shoo.shootPoint.forward = Vector3.Lerp(shoo.shootPoint.forward, aimDirection, Time.deltaTime * 20f);
        }
    }

    private void Use()
    {
       if(_inputs.use)
        {
            if(heldObject!=null)
            {
                if (highlightedInteractable != null && heldObject.canBeUsedOnType != null)
                {
                    heldObject.Drop();
                    heldObject.Use(true, highlightedInteractable);
                    Drop();
                }
                else
                {
                    heldObject.Use(true);
                }
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
        if(_inputs.interact)
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
                Drop();
            }
            _inputs.interact = false;
        }
    }

    private void Grab(Grabbable obj)
    {
        heldObject = obj;
        SetHighlightedInteractable(heldObject, false);
    }

    private void Drop()
    {
        heldObject = null;
    }

    private void OnTriggerStay(Collider collider)
    {
        //TODO add method to find closest interactable if there are multiple in range
        if (player.State == EnumPlayerState.Interacting) return;

        Interactable interactable = collider.GetComponent<Interactable>();

        if (interactable == null) return;

        if(heldObject == null)
        {
            SetHighlightedInteractable(interactable, true);
        }
        else if (heldObject.canBeUsedOnType != null && heldObject.canBeUsedOnType.IsAssignableFrom(interactable.GetType()))
        {
            SetHighlightedInteractable(interactable, true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            SetHighlightedInteractable(interactable, false);
        }
    }

    private void SetHighlightedInteractable(Interactable interactable, bool on)
    {
        if(on)
        {
            interactable.Highlight(true);
            highlightedInteractable = interactable;
        }
        else
        {
            interactable.Highlight(false);
            highlightedInteractable = null;
        }
    }

    public void DisablePlayerInteraction()
    {
        if(highlightedInteractable!=null)
        {
            SetHighlightedInteractable(highlightedInteractable, false);
        }
    }
}
