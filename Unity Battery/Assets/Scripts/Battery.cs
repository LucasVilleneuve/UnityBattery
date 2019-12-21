using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public GameObject FrontBattery;
    public Image    BatteryImage;
    public Button   button;

    public Gradient gradient;
    public float minAmmount = 1f;
    public float startingAmmount = 20f;
    public float timeToFill = 0.03f; // Number of seconds it takes to fill 1%


    // Start is called before the first frame update
    void Start()
    {
        SetScale(startingAmmount);
        SetColor();
    }

    public void Fill(float ammount) 
    {
        if (FrontBattery.transform.localScale.y < 1.0) // If battery is not full
        {
            // Add ammount to the battery
            StartCoroutine("SetScaleOverTime", FrontBattery.transform.localScale.y * 100.0f + ammount);
        }

        if (FrontBattery.transform.localScale.y >= 1.0) // If battery is full
        {
            // TODO Play animation
            Empty();
        }
    }

    public void Empty()
    {
        StartCoroutine("SetScaleOverTime", minAmmount);
        SetColor();
    }

    void SetColor (float value)  // float between 0-1
    {
        Color colorValue = gradient.Evaluate(value);
        BatteryImage.color = colorValue;
    }

    void SetColor()
    {
        SetColor(FrontBattery.transform.localScale.y);
    }

    void SetScale(float newYScale) // float between 0-100
    {
        Vector3 scale = FrontBattery.transform.localScale;
        FrontBattery.transform.localScale = new Vector3(scale.x, newYScale / 100.0f, scale.y);
    }

    IEnumerator SetScaleOverTime(float newYScale) // float between 0-100
    {
        Vector3 scale = FrontBattery.transform.localScale;
        float t = 0;
        float timeToFillScale = timeToFill * (Mathf.Abs(newYScale - scale.y * 100.0f)); // Get time to fill for this specific scale

        // Disabling button when filling
        button.interactable = false;

        while (t < 1.0f)
        {
            // Lerping scale
            t += Time.deltaTime / timeToFillScale;
            FrontBattery.transform.localScale = Vector3.Lerp(scale, new Vector3(scale.x, newYScale / 100.0f, scale.z), t);
            SetColor();

            yield return new WaitForEndOfFrame();
        }

        SetScale(newYScale);

        // Reenabling button
        button.interactable = true;
    }
}
