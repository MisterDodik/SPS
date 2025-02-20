using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScamBase : Singleton<ScamBase>
{
    GameObject canvas;

    protected Slider suspicionMeter;
    protected float difficultyLevel = 1;

    protected List<Item> possibleRewards = new List<Item>();
    [SerializeField] private GameObject stolenItems;
    protected Animator stolenItemsAnimator;

    protected virtual void Start()
    {
        possibleRewards = ItemManager.Instance.items;
        stolenItemsAnimator = stolenItems.GetComponent<Animator>();

        suspicionMeter = UIManager.Instance.GetUI<CanvasGameplay>().GetSuspicionSlider();

        ScamManager.Instance.OnScamStarted += HandleScamEvent;
    }

    protected abstract void HandleScamEvent(ScamType scamType, float npcFacing);

    protected virtual void StartTheEvent(float npc_facing, GameObject _canvas)
    {
        canvas = _canvas;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;    

        canvas.SetActive(true);
    }


    //---This can be used to give the player a random item from a list, but for now it only gives random amount of cash
    protected abstract void getItems();


    //---This shows an animation in the bottom left corner showing the stolen item
    protected virtual void showStolenItem(Item item, float amount)
    {
        stolenItems.GetComponentInChildren<Image>().sprite = item.sprite;
        stolenItems.transform.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString() + " " + item.name;

        stolenItemsAnimator.SetTrigger("stolen");
    }


    //---This can be called from anywhere, when the same scam is performed a lot of times repetitively 
    public void increaseDifficulty(float difficulty)
    {
        difficultyLevel = difficulty;
    }


    //--- If the NPC is not in range of the player, then the scam event is stopped
    public virtual void EndEvent()
    {
        if (canvas==null || !canvas.activeSelf)
            return;  

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canvas.SetActive(false);
    }
}
