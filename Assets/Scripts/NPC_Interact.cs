using UnityEngine;

public class NPC_Interact : MonoBehaviour, IInteract
{
    public void Interact()
    {
        print("Interacting");
        TextBubbleScript.instance.CreateBubble(transform, new Vector3(-0.8f, 0, 0), GetText());
    }

    public string GetText()
    {
        return "Press E to interact";
    }
}
