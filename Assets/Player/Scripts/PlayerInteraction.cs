using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            //highlightedInteractable = interactable;
            interactable.Highlight(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable!=null)
        {
            //highlightedInteractable = null;
            interactable.Highlight(false);
        }
    }
}
