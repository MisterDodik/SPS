using UnityEngine;

public class NPC_Interact : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // If dot product is negative, player is behind the NPC
        if (dotProduct < 0)
        {
            Debug.Log("Player is interacting BEHIND the NPC!");
        }
        else
        {
            Debug.Log("Player is interacting IN FRONT OF the NPC");
        }
    }
}
