using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboDisplay : MonoBehaviour
{

    private const float DISAPPEAR_TIMER_MAX = .1f;

    private TextMeshPro textMesh;
    public float disappearTimer;
    private Color textColor;
    private Color origColor;
    private Vector3 origScale;

    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        origScale = transform.localScale;
        origColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    public void setComboDisplay(string combo)
    {
        textMesh.SetText(combo);
        textMesh.color = origColor;
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        transform.localScale = origScale;
    }

    private void Update()
    {
        //float moveYSpeed = 1f;
        //transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            //First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                //Destroy(gameObject);
            }
        }
    }
}
