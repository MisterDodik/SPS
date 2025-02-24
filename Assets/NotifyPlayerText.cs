using UnityEngine;
using TMPro;
using System.Collections;

public class NotifyPlayerText : Singleton<NotifyPlayerText>
{
    private TextMeshProUGUI textObject;
    Coroutine blinkingCoroutine;
    private void Start()
    {
        textObject = UIManager.Instance.GetUI<CanvasGameplay>().getNotificationObject();
        DeleteMessage();
    }

    //---Useful when we want to inform the player that he got scammed, caught or he doesn't have enought money or anything else
    //  colorA and colorB define blinking colors of the text, text is self-explanatory
    //  hasCustomEndTime should be set to false if we want the text to be automatically disabled after the blinking is done
    // otherwise, for example if we want to delete the text once the crosshair doesnt interact with anything, then it should be set to true, and the deletion should be handled manually
    public void NotifyPlayer(Color colorA, Color colorB, string text, bool hasCustomEndTime = false)
    {
        if (textObject.gameObject.activeSelf)
            DeleteMessage();
        textObject.gameObject.SetActive(true);
        textObject.text = text;

        blinkingCoroutine = StartCoroutine(startBlinking(colorA, colorB, hasCustomEndTime));
    }
    public void DeleteMessage()
    {
        if (blinkingCoroutine != null)
            StopCoroutine(blinkingCoroutine);
        textObject.gameObject.SetActive(false);
    }

    IEnumerator startBlinking(Color A, Color B, bool hasCustomEndTime)
    {
        Color currentColor = textObject.color;

        textObject.color = Color.Lerp(currentColor, A, Time.time);
        yield return new WaitForSeconds(0.2f);
        textObject.color = Color.Lerp(A, B, Time.time);
        yield return new WaitForSeconds(0.2f);
        textObject.color = Color.Lerp(B, A, Time.time);
        yield return new WaitForSeconds(0.2f);
        textObject.color = Color.Lerp(A, B, Time.time);
        yield return new WaitForSeconds(0.2f);
        textObject.color = Color.Lerp(B, A, Time.time);

        yield return new WaitForSeconds(1.5f);
        if (!hasCustomEndTime)
            DeleteMessage();
    } 
}
