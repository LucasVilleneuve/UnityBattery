using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public GameObject FrontBattery;
    public Image    BatteryImage;
    public Button   Button;
    public Animator LeftHinge;
    public Animator RightHinge;

    public Gradient gradient;
    public float minAmmount = 1f;
    public float startingAmmount = 20f;
    public float timeToFill = 0.03f; // Number of seconds it takes to fill 1%
    public List<ColorBlock> colors;

    // Start is called before the first frame update
    void Start()
    {
        SetScale(startingAmmount);
        SetColor();
        ChangeButtonColor();
    }

    public void Fill(float ammount) 
    {
        if (FrontBattery.transform.localScale.y < 1.0) // If battery is not full
        {
            // Add ammount to the battery
            StartCoroutine(SetScaleOverTime(FrontBattery.transform.localScale.y * 100.0f + ammount));
        }

        if (FrontBattery.transform.localScale.y >= 1.0) // If battery is full
        {
            // TODO Play animation
            Empty();
        }
    }

    public void Empty()
    {
        StartCoroutine(SetScaleOverTime(minAmmount, true));
        LeftHinge.SetBool("Emptying", true);
        RightHinge.SetBool("Emptying", true);
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

    IEnumerator SetScaleOverTime(float newYScale, bool changeColor = false) // float between 0-100
    {
        Vector3 scale = FrontBattery.transform.localScale;
        float t = 0;
        float timeToFillScale = timeToFill * (Mathf.Abs(newYScale - scale.y * 100.0f)); // Get time to fill for this specific scale

        // Disabling button when filling
        Button.interactable = false;

        while (t < 1.0f)
        {
            // Lerping scale
            t += Time.deltaTime / timeToFillScale;
            FrontBattery.transform.localScale = Vector3.Lerp(scale, new Vector3(scale.x, newYScale / 100.0f, scale.z), t);
            SetColor();

            yield return new WaitForEndOfFrame();
        }

        SetScale(newYScale);
        SetColor();

        // Reenabling button
        Button.interactable = true;

        // Change Button Color
        if (changeColor) {
            ChangeButtonColor();
        }
    }

    void ChangeButtonColor()
    {
        ColorBlock newButtonColors = colors[Random.Range(0, colors.Count - 1)];
        Button.colors = newButtonColors;
        Button.GetComponentInParent<Image>().color = newButtonColors.normalColor;
    }
}
