using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected QuickOutline outline;

    protected virtual void Awake()
    {
        outline = GetComponent<QuickOutline>();
    }

    public virtual void Highlight(bool on)
    {
        if(on)
        {
            outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
            outline.outlineColor.a = 1;
        }
        else
        {
            outline.OutlineMode = QuickOutline.Mode.OutlineHidden;
            outline.outlineColor.a = 0;
        }
    }

    public virtual void Interact(Player player) {}
}
