using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class PoliceManager : Singleton<PoliceManager>
{
    [SerializeField] private List<BehaviorGraphAgent> policePrefabList = new List<BehaviorGraphAgent>();
    [SerializeField] private BehaviorGraphAgent policePrefab;
    [SerializeField] private Transform policeStation;
    private int noOfCurrentPatrolUnit = 0;
    private void Start()
    {
        
    }
    public void SendPolice(GameObject target, int numberOfCops = 1)
    {
        policePrefabList[0].SetVariableValue<bool>("IsDispatched", true);
    }
    private void SpawnPolice()
    {
        policePrefabList.Add(Instantiate(policePrefab, policeStation.position, Quaternion.identity));
    }
    
}
