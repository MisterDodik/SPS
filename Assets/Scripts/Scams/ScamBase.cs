using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScamBase : Singleton<ScamBase>
{
    GameObject baseCanvas;

    protected Slider suspicionMeter;
    protected float difficultyMultiplier = 1.2f;

    protected List<Item> possibleRewards = new List<Item>();
    [SerializeField] private GameObject stolenItems;
    protected Animator stolenItemsAnimator;

    protected float difficultyLevel = 1;

    protected ScamType currentScam = ScamType.Null;

    bool hasStarted = false;
    int multiplier=1;           
    protected virtual void Start()
    {
        possibleRewards = ItemManager.Instance.items;
        stolenItemsAnimator = stolenItems.GetComponent<Animator>();

        suspicionMeter = UIManager.Instance.GetUI<CanvasGameplay>().GetSuspicionSlider();

        ScamManager.Instance.OnScamStarted += HandleScamEvent;
    }

    protected virtual void HandleScamEvent(ScamType scamType, float npcFacing)
    {
        hasStarted = true;
        currentScam = scamType;

        multiplier = ScamManager.Instance.getDifficultyMultiplier(scamType);
        difficultyLevel = Mathf.Pow(difficultyMultiplier, multiplier);
    }

    protected virtual void StartTheEvent(float npc_facing, GameObject _canvas)
    {
        baseCanvas = _canvas;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        baseCanvas.SetActive(true);
    }


    //---This can be used to give the player a random item from a list, but for now it only gives random amount of cash
    protected abstract void getItems();


    //---This can be used once the button is clicked, ie when the QTE is over
    protected virtual void updateLastScam()
    {
        ScamManager.Instance.updateList(currentScam);
    }



    //---This shows an animation in the bottom left corner showing the stolen item
    public virtual void showStolenItem(Item item, float amount)
    {
        stolenItems.GetComponentInChildren<Image>().sprite = item.sprite;
        stolenItems.transform.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString() + " " + item.name;

        stolenItemsAnimator.SetTrigger("stolen");
    }


    //--- If the NPC is not in range of the player, then the scam event is stopped
    public virtual void EndEvent()
    {
        if (baseCanvas == null || !baseCanvas.activeSelf)
            return;  

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        baseCanvas.SetActive(false);
        hasStarted = false;
    }

    //---If the NPC is not in range of the player, then the scam event is stopped
    public virtual void ResetDifficulty()
    {
        if (!hasStarted)
            return;

        if (difficultyLevel > 1)
            difficultyLevel /= difficultyMultiplier;
        else
            difficultyLevel = 1;
    }
}
