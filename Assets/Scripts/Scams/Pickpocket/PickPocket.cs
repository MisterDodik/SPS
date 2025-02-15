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

    private void Start()
    {
        isShrinking = false;
        canvas.SetActive(false);
        suspicionMeter = UIManager.Instance.GetUI<CanvasGameplay>().GetSuspicionSlider();

        innerCircleSize = innerCircle.rectTransform.sizeDelta.x;
    }
    public void StartTheEvent(float npc_facing)
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
        if (outerCircleSize > 0 && isShrinking)
        {
            outerCircleSize -= Time.deltaTime * difficultyConstant * difficultyLevel;

            outerCircle.rectTransform.sizeDelta = new Vector2(outerCircleSize, outerCircleSize);
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
        float moneyAmount = Random.Range(15, 50) / difficultyLevel;        // dividing by difficultyLevel because that shows how often the scam was perfomed lately
        moneyAmount = (Mathf.Round(moneyAmount * 100)) / 100;    //rounding to 2 decimal places

        Player.Instance.ChangeMoney(moneyAmount);       
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
