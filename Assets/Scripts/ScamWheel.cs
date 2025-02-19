using UnityEngine;

public class ScamWheel : MonoBehaviour
{
    GameObject ScamWheelGO;

    private void Start()
    {
        ControlsManager.Instance.OnScamWheelActivate += Instance_OnScamWheelActivate;
        ControlsManager.Instance.OnScamWheelDisable += Instance_OnScamWheelDisable;

        ScamWheelGO = UIManager.Instance.GetUI<CanvasGameplay>().GetScamWheel();

        ScamWheelGO.SetActive(false);

    }

    private void Instance_OnScamWheelDisable(object sender, System.EventArgs e)
    {
        ScamWheelGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Instance_OnScamWheelActivate(object sender, System.EventArgs e)
    {
        ScamWheelGO.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
