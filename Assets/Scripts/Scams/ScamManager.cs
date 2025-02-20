using System;
using Unity.Behavior;
using UnityEngine;

public class ScamManager : Singleton<ScamManager>
{
    public event Action<ScamType, float, bool> OnScamStarted;
    private BehaviorGraphAgent affectedNPC;
 
    public void StartScam(ScamType scamType, float dependency, BehaviorGraphAgent affectedNPC, bool isRepeated)
    {
        this.affectedNPC = affectedNPC;
        OnScamStarted?.Invoke(scamType, dependency, isRepeated);
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