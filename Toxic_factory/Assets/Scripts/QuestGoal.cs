using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public enum GoalType { CloseValve, GatherItem }
    public GoalType goalType;

    public int item;
    public int info;

    public bool IsReached()
    {
        return item >= info;
    }

    public void ItemCollected()
    {
        if (goalType == GoalType.GatherItem)
            item++;
    }

    public void InfoCollected()
    {
        if (goalType == GoalType.CloseValve)
            info++;
    }
}
