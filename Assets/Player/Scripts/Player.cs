using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerController _controller;
    [SerializeField]
    private PlayerInteraction _interaction;
    [SerializeField]
    private PlayerInputs _inputs;

    public PlayerController Controller => _controller;
    public PlayerInteraction Interaction => _interaction;
    public PlayerInputs Inputs => _inputs;

    [SerializeField]
    public Transform grabTransform;

    EnumPlayerInteractionState _stateInteraction = EnumPlayerInteractionState.Free;
    public EnumPlayerInteractionState StateInteraction
    {
        get => _stateInteraction;
        set
        {
            _stateInteraction = value;
            switch (_stateInteraction)
            {
                case EnumPlayerInteractionState.Free:
                    _inputs.interact = false;
                    _interaction.enabled = true;
                    _inputs.EnablePlayerInput(true);
                    break;
                case EnumPlayerInteractionState.Interacting:
                    _inputs.EnablePlayerInput(false);
                    _interaction.DisablePlayerInteraction();
                    _interaction.enabled = false;
                    break;
            }
        }
    }

    EnumPlayerAimState _stateAim = EnumPlayerAimState.Free;
    public EnumPlayerAimState StateAim
    {
        get => _stateAim;
        set
        {
            _stateAim = value;
            switch (_stateAim)
            {
                case EnumPlayerAimState.Free:
                    break;
                case EnumPlayerAimState.Aiming:
                    break;
            }
        }
    }
}

public enum EnumPlayerInteractionState
{
    Free,
    Interacting,
}

public enum EnumPlayerAimState
{
    Free,
    Aiming,
}