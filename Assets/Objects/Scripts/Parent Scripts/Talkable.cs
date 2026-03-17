using UnityEngine;
using Yarn.Unity;

public class Talkable : Interactable
{
    [SerializeField]
    protected string dialogueNode;
    public virtual void Talk()
    {
        GameManager.Instance.dialogueRunner.StartDialogue(dialogueNode);
        GameManager.Instance.player.StateInteraction = EnumPlayerInteractionState.Interacting;
        GameManager.Instance.dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    private void OnDialogueComplete()
    {
        GameManager.Instance.player.StateInteraction = EnumPlayerInteractionState.Free;
        GameManager.Instance.dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueComplete);
    }
}
