using Unity.Behavior;
using UnityEngine;

public class NPC_Interact : MonoBehaviour, IInteractable
{
    [SerializeField] private BehaviorGraphAgent agent;
    ScamType lastScam = ScamType.Null;
    public void Interact(Player player)
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        ScamType scamType = ScamWheel.Instance.selectedScam;
        
        // If dot product is negative, player is behind the NPC
        if (dotProduct < 0)
        {
            //Player is interacting BEHIND the NPC
            ScamManager.Instance.StartScam(scamType, 1, agent, scamType==lastScam ? true : false);
        }
        else
        {
            //Player is interacting IN FRONT OF the NPC
            ScamManager.Instance.StartScam(scamType, 1.2f, agent, scamType == lastScam ? true : false);
        }
        lastScam = scamType;
    }
}
