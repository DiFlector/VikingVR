using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Quest{
    public string name;
    public string description;
    public string condition;
    public int count;
    public int reward;
    public int nowCount = 0;
    public bool isDone = false;
    public bool isReward = false;

    public Quest(string name_ = "", string description_ = "", string condition_ = "", int count_ = 0, int reward_ = 0)
    {
        name = name_;
        description = description_;
        condition = condition_;
        count = count_;
        reward = reward_;
    }

    public void Condition()
    {
        if(nowCount == count)
        {
            isReward = true;
        }
    }

    public void TakeReward()
    {
        isDone = true;
    }

    
}

public class QuestEngine : MonoBehaviour
{
    List<Quest> QuestList;
    public int questId;
    public TextMeshPro QuestNameMesh, QuestDescriptionMesh, QuestConditionMesh, QuestCountMesh;
    public VillagerEngine villager;

    void Start()
    {
        QuestList = new List<Quest>();
        QuestAdd();
    }

    void Update()
    {
        updateInfo();
        QuestList[questId].Condition();
    }

    void updateInfo()
    {
        QuestNameMesh.text = QuestList[questId].name;
        QuestDescriptionMesh.text = QuestList[questId].description;
        QuestConditionMesh.text = QuestList[questId].condition;
        if (!QuestList[questId].isReward && !QuestList[questId].isDone)
        {
            QuestCountMesh.text = QuestList[questId].nowCount.ToString() + "/" + QuestList[questId].count.ToString();
        }
        else if (QuestList[questId].isReward && !QuestList[questId].isDone)
        {
            QuestCountMesh.text = "Get reward";
        }
        else
        {
            QuestCountMesh.text = "Done";
        }
    }

    void takeQuest(int id)
    {
        questId = id;
    }

    void QuestAdd()
    {
        QuestList.Add(new Quest("QustName", 
            "QuestDescription",
            "QuestCondition",
            10, 10));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<VillagerEngine>(out villager))
        {
            QuestList[questId].TakeReward();
        }
        
    }
}

