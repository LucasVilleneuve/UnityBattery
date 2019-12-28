using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public GameObject FrontBattery;
    public Renderer BatteryGaugeRenderer;
    public Button   Button;
    public Animator LeftHinge;
    public Animator RightHinge;

    public Gradient gradient;
    [Range(0, 100)]
    public float minAmmount = 1f;
    [Range(0, 100)]
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
        if (FrontBattery.transform.localScale.z < 1.0) // If battery is not full
        {
            // Add ammount to the battery
            StartCoroutine(SetScaleOverTime(FrontBattery.transform.localScale.z * 100.0f + ammount));
        }

        if (FrontBattery.transform.localScale.z >= 1.0) // If battery is full
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
        BatteryGaugeRenderer.material.color = colorValue;
    }

    void SetColor()
    {
        SetColor(FrontBattery.transform.localScale.z);
    }

    void SetScale(float newZScale) // float between 0-100
    {
        Vector3 scale = FrontBattery.transform.localScale;
        FrontBattery.transform.localScale = new Vector3(scale.x, scale.y, newZScale / 100.0f);
    }

    IEnumerator SetScaleOverTime(float newZScale, bool changeColor = false) // float between 0-100
    {
        Vector3 scale = FrontBattery.transform.localScale;
        float t = 0;
        float timeToFillScale = timeToFill * (Mathf.Abs(newZScale - scale.z * 100.0f)); // Get time to fill for this specific scale

        // Disabling button when filling
        Button.interactable = false;

        while (t < 1.0f)
        {
            // Lerping scale
            t += Time.deltaTime / timeToFillScale;
            FrontBattery.transform.localScale = Vector3.Lerp(scale, new Vector3(scale.x, scale.y, newZScale / 100.0f), t);
            SetColor();

            yield return new WaitForEndOfFrame();
        }

        SetScale(newZScale);
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
