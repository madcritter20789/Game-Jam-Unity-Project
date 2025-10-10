using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerController player;
    public QuestUI questUI;

    public void OpenQuestWindow()
    {
        questUI.ShowQuest(quest, this);
    }

    public void AcceptQuest()
    {
        quest.isActive = true;
        player.currentQuest = quest;
        questUI.HideQuestWindow();
        questUI.UpdateQuestProgress(quest);
    }
}
