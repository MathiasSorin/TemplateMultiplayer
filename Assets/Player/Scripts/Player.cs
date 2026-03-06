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
}