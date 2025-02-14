using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.Open<CanvasMainMenu>();
        UIManager.Instance.Open<CanvasGameplay>();

    }
}
