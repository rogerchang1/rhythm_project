using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccuracyPopup : MonoBehaviour
{
    
    //Create a Accuracy Popup
    public static AccuracyPopup Create(Vector3 position, float accuracyAmount)
    {
        Transform accuracyPopupTransform = Instantiate(GameAssets.i.AccuracyPopup, position, Quaternion.identity);
        AccuracyPopup accuracyPopup = accuracyPopupTransform.GetComponent<AccuracyPopup>();
        accuracyPopup.Setup(accuracyAmount);

        return accuracyPopup;
    }

    private const float DISAPPEAR_TIMER_MAX = .1f;

    private static int sortingOrder;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

        
    }
    public void Setup(float accuracy)
    {
        textMesh.SetText(accuracy.ToString());
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
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
        if(disappearTimer < 0)
        {
            //Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a <0)
            {
                Destroy(gameObject);
            }
        }
    }
}
