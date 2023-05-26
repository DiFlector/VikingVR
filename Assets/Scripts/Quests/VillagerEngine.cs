using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VillagerEngine : MonoBehaviour
{
    public string name;
    private int currentQuest = 0;
    public int[] Quests;
    public TextMeshPro Title, Description, Button;
    public string TitleText, DescriptionText, ButtonText;

    private void Start()
    {
        Title.text = TitleText;
        Description.text = DescriptionText;
        Button.text = ButtonText;
    }

    void questReward()
    {
        currentQuest++;
    }
}
