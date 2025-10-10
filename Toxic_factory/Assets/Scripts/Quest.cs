using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questTitle;
    public string description;
    public int experienceReward;
    public int HazzardLevel;

    public bool isActive;
    public QuestGoal goal;

    public void Complete(PlayerController player)
    {
        Debug.Log($"Quest Completed: {questTitle}");
        isActive = false;

        // Grant rewards
        player.experience += experienceReward;
        player.toxicityLevel += HazzardLevel;
    }
}
