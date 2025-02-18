using System;
using UnityEngine;

public class ScamManager : Singleton<ScamManager>
{
    public event Action<ScamType, float> OnScamStarted;
 
    public void StartScam(ScamType scamType, float dependency)
    {
        OnScamStarted?.Invoke(scamType, dependency);
    }
}
public enum ScamType
{
    Pickpocket,
    Distraction
}