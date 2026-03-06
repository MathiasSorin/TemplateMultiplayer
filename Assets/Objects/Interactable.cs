using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Outline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        if(outline==null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
    }

    public virtual void Highlight(bool on)
    {
        if(on)
        {
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.outlineColor.a = 1;
        }
        else
        {
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            outline.outlineColor.a = 0;
        }
    }

    public virtual void Interact(Player player) {}
}
