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
                    _controller.enabled = true;
                    _interaction.enabled = true;
                    break;
                case EnumPlayerState.Interacting:
                    _controller.enabled = false;
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