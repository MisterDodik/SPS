
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : ScamBase
{
    [SerializeField] GameObject canvas;

    [SerializeField] private Image arrow;

    float interval = 0;

    bool leftToRight = false;
    Vector3 startPos = new Vector3(-150, -150);
    Vector3 endPos = new Vector3(150, -150);

    Quaternion leftRot = Quaternion.Euler(0, 0, 90);
    Quaternion rightRot = Quaternion.Euler(0, 0, -90);


    private float difficultyConstant = 120;
    private float npcFacingMultiplier = 1;
    protected override void Start()
    {
        base.Start();

        canvas.SetActive(false);
    }
    protected override void HandleScamEvent(ScamType scamType, float npcFacing, bool isRepeated)
    {
        base.HandleScamEvent(scamType, npcFacing, isRepeated);
        print(difficultyLevel);

        if (scamType == ScamType.Distraction)
        {
            npcFacingMultiplier = npcFacing;
            StartTheEvent(npcFacing, canvas);
        }
    }
    private void Update()
    {
        interval+=Time.deltaTime;
        Vector3 center = (startPos + endPos) / 2;

        // Move the center a bit downwards to make the arc vertical
        center -= new Vector3(0, 1, 0);

        Vector3 startRelCenter = startPos - center;
        Vector3 endRelCenter = endPos - center;

        if (leftToRight)
        {
            arrow.rectTransform.anchoredPosition = Vector3.Slerp(startRelCenter, endRelCenter, interval);
            arrow.rectTransform.anchoredPosition += (Vector2)center;

            arrow.rectTransform.rotation = Quaternion.Slerp(leftRot, rightRot, interval);
            if (Vector2.Distance(arrow.rectTransform.anchoredPosition, endPos) < 0.1f)
            {
                interval = 0;
                leftToRight = false;
            }
        }
        else
        {
            arrow.rectTransform.anchoredPosition = Vector3.Slerp(endRelCenter, startRelCenter, interval);
            arrow.rectTransform.anchoredPosition +=(Vector2) center;

            arrow.rectTransform.rotation = Quaternion.Slerp(rightRot, leftRot, interval);
            if (Vector2.Distance(arrow.rectTransform.anchoredPosition, startPos) < 0.1f)
            {
                interval = 0;
                leftToRight = true;
            }
        }
    }


    public void OnClick()
    {
        float currentPos = arrow.rectTransform.eulerAngles.z;
        currentPos = Mathf.Repeat(currentPos + 180, 360) - 180;
        currentPos = Mathf.Abs(currentPos);

        if (currentPos < 12)
            getItems();
        else if (currentPos > 30 && currentPos < 60)
        {
            suspicionMeter.value += difficultyLevel * npcFacingMultiplier * currentPos / 10;
            getItems();
        }
        else
            suspicionMeter.value += difficultyLevel * npcFacingMultiplier * currentPos / 5;

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
}
