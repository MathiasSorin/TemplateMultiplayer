using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public string description;
    public bool completed = false;

    public void Complete()
    {
        completed = true;
    }
}
