using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class PoliceManager : Singleton<PoliceManager>
{
    [SerializeField] private List<BehaviorGraphAgent> policeList = new List<BehaviorGraphAgent>();
    [SerializeField] private BehaviorGraphAgent policePrefab;
    [SerializeField] private Transform policeStation;
    private int noOfCurrentPatrolUnit = 0;
    private void Start()
    {
        
    }
    public void SendPolice(GameObject target, Vector3 reportedPosition, int numberOfCops = 1)
    {
        policeList.Sort((a, b) =>
            Vector3.Distance(a.transform.position, reportedPosition)
            .CompareTo(Vector3.Distance(b.transform.position, reportedPosition)));

        // Send the closest N officers
        for (int i = 0; i < Mathf.Min(numberOfCops, policeList.Count); i++)
        {
            policeList[i].SetVariableValue<bool>("IsDispatched", true);
            Debug.Log($"Dispatched: {policeList[i].name} to {reportedPosition}");
        }
    }
    private void SpawnPolice()
    {
        policeList.Add(Instantiate(policePrefab, policeStation.position, Quaternion.identity));
    }
    
}
