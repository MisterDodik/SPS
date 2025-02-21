using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class ScamManager : Singleton<ScamManager>
{
    public event Action<ScamType, float> OnScamStarted;
    private BehaviorGraphAgent affectedNPC;


    List<ScamType> lastScams = new List<ScamType>();
    private Dictionary<ScamType, int> scamDifficulties = new();
    int _threshold = 3;         //how many different scams have to be performed before resetting the difficulty level of a certain scam
    public int getDifficultyMultiplier(ScamType currentType)
    {
        int currentTypeIndex = 0;           //index of the currentType in the list
        for (int i = 0; i < lastScams.Count; i++)
        {
            if (lastScams[i] == currentType)
                currentTypeIndex = i;
        }

        int differentAfter = 0;             //counts from the end to the start how many types are there
        bool flag = false;                  //if we encounter the same type it means the currentType was not the last usage of the scam, so it would look like this ABCDEGD(2 D's)
        for (int i = lastScams.Count - 1; i > currentTypeIndex; i--)
        {
            if (lastScams[i] != currentType)
                differentAfter++;
            else
            {
                flag = true;
                break;
            }
        }
    
        if (differentAfter >= _threshold && !flag)          //enough different scams have been performed and none of them was the same as currentType
        {
            if (scamDifficulties.ContainsKey(currentType))          //resets values in the dictionary
                scamDifficulties[currentType] = 0;
            else
                scamDifficulties.Add(currentType, 0);
        }

        return scamDifficulties.TryGetValue(currentType, out int difficultyPower) ? difficultyPower : 0;
    }

    //---Called after the performed scam
    public void updateList(ScamType type)
    {
        if (!scamDifficulties.ContainsKey(type))
            scamDifficulties.Add(type, 1);      // not 0 because it has already been used, right before this function was called
        else
            scamDifficulties[type]++;
        
        lastScams.Add(type);

        if (lastScams.Count > 30)
            lastScams.RemoveAt(0);
    }





    public void StartScam(ScamType scamType, float dependency, BehaviorGraphAgent affectedNPC)
    {
        this.affectedNPC = affectedNPC;
        OnScamStarted?.Invoke(scamType, dependency);
    }
    public BehaviorGraphAgent GetAffectedNPC()
    {
        return affectedNPC;
    }
}
public enum ScamType
{
    Null,
    Pickpocket,
    Distraction,
    ATM,
    FakeTickets
}