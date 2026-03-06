using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;
    private PlayerInputs _input;

    private Interactable highlightedInteractable;
    private Grabbable grabbable;

    public float throwForce = 10f;

    void Start()
    {
        player = GetComponent<Player>();
        _input = player.Inputs;
    }

    void Update()
    {
        Interaction();
    }

    private void Interaction()
    {
        if(_input.interact)
        {
            if(highlightedInteractable!=null)
            {
                if (highlightedInteractable.GetComponent<Grabbable>() is Grabbable obj)
                {
                    grabbable = obj;
                }
                highlightedInteractable.Interact(player);
            }
            else if(grabbable!=null)
            {
                grabbable.Throw(player.grabTransform.forward, throwForce);
                grabbable = null;
            }
            _input.interact = false;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            highlightedInteractable = interactable;
            interactable.Highlight(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            highlightedInteractable = null;
            interactable.Highlight(false);
        }
    }
}
