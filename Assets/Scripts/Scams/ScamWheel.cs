using UnityEngine;

public class ScamWheel : Singleton<ScamWheel>
{
    [HideInInspector]public ScamType selectedScam = ScamType.Pickpocket;
    
    GameObject ScamWheelGO;

    public ScamBase scam;
    private void Start()
    {
        ControlsManager.Instance.OnScamWheelActivate += Instance_OnScamWheelActivate;
        ControlsManager.Instance.OnScamWheelDisable += Instance_OnScamWheelDisable;

        ScamWheelGO = UIManager.Instance.GetUI<CanvasGameplay>().GetScamWheel();

        ScamWheelGO.SetActive(false);
    }

    public void getcurrentScam(ScamBase _scam)
    {
        scam = _scam;
        print(scam);
    }
    private void Instance_OnScamWheelDisable(object sender, System.EventArgs e)
    {
        ScamWheelGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        print(selectedScam);
    }

    private void Instance_OnScamWheelActivate(object sender, System.EventArgs e)
    {
        ScamWheelGO.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void pickScam(ScamType type)
    {
        selectedScam = type;
    }
}
