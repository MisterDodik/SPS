using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Notify", story: "Agent shows a [Message] to the [Target]", category: "Action", id: "71a2fe08f3faa912ae7b774d2dfd7462")]
public partial class NotifyAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Message;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        NotifyPlayerText.Instance.NotifyPlayer(new Color(1, 0.3f, 0), new Color(1, 0, 0), Message);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

