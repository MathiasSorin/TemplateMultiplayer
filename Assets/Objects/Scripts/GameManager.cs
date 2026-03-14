using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    public DialogueRunner dialogueRunner;
    [SerializeField]
    public Player player;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindAnyObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
