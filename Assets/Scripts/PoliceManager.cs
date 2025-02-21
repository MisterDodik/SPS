using Unity.Behavior;
using UnityEngine;

public class PoliceManager : Singleton<PoliceManager>
{
    [SerializeField] private BehaviorGraphAgent[] policePrefabArray;

    [SerializeField] private Transform policeStation;

    private void Start()
    {
        
    }
    public void SendPolice(GameObject target, int numberOfCops = 1)
    {
        policePrefabArray[0].SetVariableValue<bool>("IsDispatched", true);
    }
}
