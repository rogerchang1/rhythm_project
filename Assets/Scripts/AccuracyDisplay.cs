using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccuracyDisplay : MonoBehaviour
{

    private const float DISAPPEAR_TIMER_MAX = .1f;

    private TextMeshPro textMesh;
    public float disappearTimer;
    private Color textColor;
    private Color origColor;
    private Vector3 origScale;
    private Vector3 origPosition;

    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        origScale = transform.localScale;
        origPosition = transform.position;
        origColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        textColor.a = 0;
    }

    public void setAccuracyDisplay(string accuracy)
    {
        textMesh.SetText(accuracy);
        textMesh.color = origColor;
        textColor = textMesh.color;
        
        disappearTimer = DISAPPEAR_TIMER_MAX;
        transform.position = origPosition;
        transform.localScale = origScale;
    }

    private void Update()
    {
        float moveYSpeed = 1f;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            //First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else if (disappearTimer > 0)
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        if (disappearTimer > 0)
        {
            disappearTimer -= Time.deltaTime;
            transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        }
        else if (textColor.a > 0)
        {
            //Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        }
    }
}
