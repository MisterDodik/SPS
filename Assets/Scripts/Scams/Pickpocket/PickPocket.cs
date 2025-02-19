using UnityEngine;
using UnityEngine.UI;

public class PickPocket : ScamBase
{
    [SerializeField] new GameObject canvas;

    [SerializeField] private Image outerCircle;
    [SerializeField] private Image innerCircle;
    private float outerCircleSize;
    private float innerCircleSize;

    private bool isShrinking = false;
    private float difficultyConstant = 120;
    private float npcFacingMultiplier = 1;


    protected override void Start()
    {
        base.Start();

        canvas.SetActive(false);

        isShrinking = false;

        innerCircleSize = innerCircle.rectTransform.sizeDelta.x;
    }

    protected override void HandleScamEvent(ScamType scamType, float npcFacing)
    {
        if (scamType == ScamType.Pickpocket)
        {
            npcFacingMultiplier = npcFacing;
            StartTheEvent(npcFacing, canvas);
        }
    }

    protected override void StartTheEvent(float npc_facing, GameObject _canvas)
    {
        base.StartTheEvent(npc_facing, _canvas);

        npcFacingMultiplier = npc_facing;

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

        EndEvent();
    }
    
    //---This can be used to give the player a random item from a list, but for now it only gives random amount of cash
    protected override void getItems()
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


    //--- If the NPC is not in range of the player, then the scam event is stopped
    public override void EndEvent()
    {
        base.EndEvent();
        isShrinking = false;
        outerCircle.gameObject.SetActive(false);
    }
}
