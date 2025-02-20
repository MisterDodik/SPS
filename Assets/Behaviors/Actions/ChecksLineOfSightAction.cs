using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Checks Line Of Sight", story: "[Agent] checks for [Target] in line of sight", category: "Action", id: "a99051eba2d8aa6fd459e35f5314a0cc")]
public partial class ChecksLineOfSightAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> detectionRadius;
    [SerializeReference] public BlackboardVariable<float> fieldOfViewAngle;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        GameObject agentObj = Agent.Value;
        Transform targetTransform = Target.Value;

        Vector3 agentPos = agentObj.transform.position;
        Vector3 targetPos = targetTransform.position;
        Vector3 toTarget = (targetPos - agentPos).normalized;
        float distanceToTarget = Vector3.Distance(agentPos, targetPos);

        // 1. Check if the player is within the NPC's vision range
        if (distanceToTarget > detectionRadius)
        {
            Debug.Log("Out Of Range");
            return Status.Failure;
        }

        // 2. Check if the player is within the NPC's field of view angle
        Vector3 agentForward = agentObj.transform.forward;
        float angleToTarget = Vector3.Angle(agentForward, toTarget);

        if (angleToTarget > fieldOfViewAngle / 2)
        {
            Debug.Log("Out Of FOV");
            return Status.Failure; // Outside FOV
        }

        // 3. Check if there's an obstacle blocking the view
        Collider[] colliders = Physics.OverlapSphere(agentPos, distanceToTarget);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
                continue; // Ignore player, check obstacles only

            Vector3 toObstacle = (col.transform.position - agentPos).normalized;
            float angleToObstacle = Vector3.Angle(agentForward, toObstacle);

            // Debugging
            Debug.Log($"Checking {col.name}, Angle: {angleToObstacle}");

            if (angleToObstacle < fieldOfViewAngle / 2) // If inside the vision cone
            {
                // Raycast to verify actual line of sight blocking
                int obstacleLayerMask = ~LayerMask.GetMask("Ground"); // Ignore ground collisions
                if (Physics.Linecast(agentPos, col.transform.position, out RaycastHit hit, obstacleLayerMask))
                {
                    if (hit.collider == col) // Only fail if the obstacle is actually in the way
                    {
                        Debug.Log($"Blocked by: {col.name}");
                        return Status.Failure; // Vision is blocked
                    }
                }
            }
        }
        return Status.Success; // Player is visible!
    }

    protected override void OnEnd()
    {
    }
}

