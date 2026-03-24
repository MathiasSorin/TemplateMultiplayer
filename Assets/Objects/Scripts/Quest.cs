using UnityEngine;

public class Quest : MonoBehaviour
{
    private string description;
    private QuestObjective[] questObjectives;

    private void CompleteQuestObjective(QuestObjective questObjective)
    {
        questObjective.Complete();
        CheckQuestCompletion();
    }

    private void CheckQuestCompletion()
    {
        foreach (var questObj in questObjectives)
        {
            if(!questObj.completed)
            {
                return;
            }
        }

        Debug.Log("WIN");
    }
}
