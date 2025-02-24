using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Target In FOV", story: "[Agent] checks for [Target] [isIn] [Range] and [FOV]", category: "Conditions", id: "d04b26a69e24dadeb6aea5ae5a0cc747")]
public partial class CheckTargetInFovCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<float> FOV;
    [SerializeReference] public BlackboardVariable<bool> isIn;

    public override bool IsTrue()
    {
        GameObject agentObj = Agent.Value;
        Transform targetTransform = Target.Value.transform;

        Vector3 agentPos = agentObj.transform.position;
        Vector3 targetPos = targetTransform.position;
        float distanceToTarget = Vector3.Distance(agentPos, targetPos);

        // 1. Check if the player is within the NPC's vision range
        if (distanceToTarget > Range)
        {
            Debug.Log("Out Of Range");
            return isIn ? false : true;
        }
        Vector3 flatAgentPos = new Vector3(agentPos.x, 0, agentPos.z);
        Vector3 flatTargetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 toTargetFlat = (flatTargetPos - flatAgentPos).normalized;
        Vector3 agentForward = agentObj.transform.forward;

        float angleToTarget = Vector3.Angle(new Vector3(agentForward.x, 0, agentForward.z), toTargetFlat);
        // 2. Check if the player is within the NPC's field of view angle
        if (angleToTarget > FOV / 2)
        {
            Debug.Log("Out Of FOV");
            return isIn ? false : true;
        }

        // 3. Check if there's an obstacle blocking the view
        Collider[] colliders = Physics.OverlapSphere(agentPos, distanceToTarget);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
                continue; // Ignore player, check obstacles only

            Vector3 toObstacle = (col.transform.position - agentPos).normalized;
            float angleToObstacle = Vector3.Angle(agentForward, toObstacle);

            if (angleToObstacle < FOV / 2) // If inside the vision cone
            {
                int obstacleLayerMask = ~LayerMask.GetMask("Ground"); // Ignore ground collisions
                if (Physics.Linecast(agentPos, col.transform.position, out RaycastHit hit, obstacleLayerMask))
                {
                    if (hit.collider == col)
                    {
                        Debug.Log($"Blocked by: {col.name}");
                        return isIn ? false : true; // Vision is blocked
                    }
                }
            }
        }
        return isIn ? true : false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
