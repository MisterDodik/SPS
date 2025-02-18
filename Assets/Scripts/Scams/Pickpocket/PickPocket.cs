using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickPocket : Singleton<PickPocket>
{
    [SerializeField] GameObject canvas;

    [SerializeField] Image outerCircle;
    [SerializeField] Image innerCircle;
    private float outerCircleSize;
    private float innerCircleSize;

    Slider suspicionMeter;

    [SerializeField] private float npcFacingMultiplier = 1;     // It's better to approach npc from behind
    
    private float difficultyConstant = 120;
    [SerializeField] private float difficultyLevel;             // The more the same scam is performed the more difficult the scam gets

    bool isShrinking = true;


    List<Item> possibleRewards = new List<Item>();
    [SerializeField] public GameObject stolenItems;
    Animator stolenItemsAnimator;

    private void Start()
    {
        possibleRewards = ItemManager.Instance.items;
        stolenItemsAnimator = stolenItems.GetComponent<Animator>();

        isShrinking = false;
        canvas.SetActive(false);
        suspicionMeter = UIManager.Instance.GetUI<CanvasGameplay>().GetSuspicionSlider();

        innerCircleSize = innerCircle.rectTransform.sizeDelta.x;

        ScamManager.Instance.OnScamStarted += HandleScamEvent;
    }

    private void HandleScamEvent(ScamType scamType, float npcFacing)
    {
        if (scamType == ScamType.Pickpocket)
        {
            StartTheEvent(npcFacing);
        }
    }

    void StartTheEvent(float npc_facing)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        npcFacingMultiplier = npc_facing;

        canvas.SetActive(true);
        outerCircle.gameObject.SetActive(true);

        outerCircleSize = 500;
        outerCircle.rectTransform.sizeDelta = new Vector2(500, 500);

        isShrinking = true;
    }

    private void Update()
    {
        if (isShrinking)
        {
            if (outerCircleSize > 0)
            {
                outerCircleSize -= Time.deltaTime * difficultyConstant * difficultyLevel;

                outerCircle.rectTransform.sizeDelta = new Vector2(outerCircleSize, outerCircleSize);
            }
            else
            {
                ButtonPressed();
            }
        }       
    }

    public void ButtonPressed()
    {
        isShrinking = false;
        outerCircle.gameObject.SetActive(false);

        float distance = outerCircleSize - innerCircleSize;
        distance = Mathf.Abs(distance);

        if (distance < 5)
            getItems();
        else if (distance < 12)
        {
            suspicionMeter.value += difficultyLevel * npcFacingMultiplier * distance / 20;
            getItems();
        }
        else
            suspicionMeter.value += difficultyLevel * npcFacingMultiplier * distance / 10;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canvas.SetActive(false);
    }
    
    //---This can be used to give the player a random item from a list, but for now it only gives random amount of cash
    void getItems()
    {
        Item item = possibleRewards[Random.Range(0, possibleRewards.Count)];

        if (item is not CashItem)
        {
            InventorySystem.Instance.AddItem(item, +1);
            showStolenItem(item, 1);
            return;
        }

        float moneyAmount = Random.Range(15, 50) / difficultyLevel;        // dividing by difficultyLevel because that shows how often the scam was perfomed lately
        moneyAmount = (Mathf.Round(moneyAmount * 100)) / 100;    //rounding to 2 decimal places
        showStolenItem(item, moneyAmount);

        Player.Instance.ChangeMoney(moneyAmount);       
    }

    //---This shows an animation in the bottom left corner showing the stolen item
    void showStolenItem(Item item, float amount)
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
    public void EndEvent()
    {
        if (!canvas.activeSelf)
            return;

        isShrinking = false;
        outerCircle.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canvas.SetActive(false);
    }
}
