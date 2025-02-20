using System;
using Unity.Behavior;
using UnityEngine;

public class ScamManager : Singleton<ScamManager>
{
    public event Action<ScamType, float> OnScamStarted;
    private BehaviorGraphAgent affectedNPC;
 
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
    Pickpocket,
    Distraction
}