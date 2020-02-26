using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Color textColor;
    private Color origColor;
    private Vector3 origScale;
    private const float POP_TIMER_MAX = 1f;
    private float popTimer;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        origScale = transform.localScale;
        origColor = textMesh.color;
        popTimer = 0;
        textMesh.SetText("Score = 0");
    }

    public void setScoreDisplay(float score)
    {
        textMesh.SetText("Score = "+score.ToString());
        textMesh.color = origColor;
        textColor = textMesh.color;
        transform.localScale = origScale;
        popTimer = POP_TIMER_MAX;
    }

    // Update is called once per frame
    void Update()
    {
      /*  popTimer -= Time.deltaTime;
        if (popTimer > POP_TIMER_MAX * .5f)
        {
            //First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else if (popTimer > 0)
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }*/
    }
}
