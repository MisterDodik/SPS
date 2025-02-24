using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate to Target", story: "[Agent] rotates to [Target]", category: "Action", id: "6a1beea52e5a3b9136a4263eb8838650")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> RotationSpeed; // Speed of rotation

    private Transform agentTransform;
    private Quaternion targetRotation;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null) return Status.Failure;

        agentTransform = Agent.Value.transform;

        // Calculate initial flat direction (ignore Y-axis)
        Vector3 agentPos = agentTransform.position;
        Vector3 targetPos = Target.Value.transform.position;

        Vector3 flatAgentPos = new Vector3(agentPos.x, 0, agentPos.z);
        Vector3 flatTargetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 targetDirection = (flatTargetPos - flatAgentPos).normalized;

        targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        agentTransform.rotation = Quaternion.RotateTowards(agentTransform.rotation, targetRotation, RotationSpeed.Value * Time.deltaTime);

        float angleDifference = Quaternion.Angle(agentTransform.rotation, targetRotation);

        if (angleDifference < 1f)
            return Status.Success;

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}
