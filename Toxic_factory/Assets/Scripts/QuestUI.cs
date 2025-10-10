using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("Quest Window UI")]
    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;

    [Header("HUD Quest Display")]
    public TextMeshProUGUI currentQuestText;

    private QuestGiver activeQuestGiver;

    public void ShowQuest(Quest quest, QuestGiver giver)
    {
        questWindow.SetActive(true);
        titleText.text = quest.questTitle;
        descriptionText.text = quest.description;
        rewardText.text = $"XP: {quest.experienceReward}, Gold: {quest.experienceReward}";
        activeQuestGiver = giver;
    }

    public void AcceptQuest()
    {
        activeQuestGiver.AcceptQuest();
    }

    public void HideQuestWindow()
    {
        questWindow.SetActive(false);
    }

    public void UpdateQuestProgress(Quest quest)
    {
        if (quest != null && quest.isActive)
        {
            currentQuestText.text =
                $"{quest.questTitle} ({quest.goal.info}/{quest.goal.info})";
        }
        else
        {
            currentQuestText.text = "No Active Quest";
        }
    }
}
