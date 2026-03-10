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

    EnumPlayerState _state = EnumPlayerState.Free;
    public EnumPlayerState State
    {
        get => _state;
        set
        {
            _state = value;
            switch (_state)
            {
                case EnumPlayerState.Free:
                    _inputs.interact = false;
                    _interaction.enabled = true;
                    _inputs.EnablePlayerInput(true);
                    break;
                case EnumPlayerState.Interacting:
                    _inputs.EnablePlayerInput(false);
                    _interaction.DisablePlayerInteraction();
                    _interaction.enabled = false;
                    break;
            }
        }
    }
}

public enum EnumPlayerState
{
    Free,
    Interacting
}