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
        textMesh.SetText("Score = " + 0
                        + "\n !MIKAN! = " + 0
                        + "\n 90 = " + 0
                        + "\n 80 = " + 0
                        + "\n 70 = " + 0
                        + "\n 60 = " + 0
                        + "\n 50 = " + 0
                        + "\n 40 = " + 0
                        + "\n 30 = " + 0
                        + "\n 20 = " + 0
                        + "\n 10 = " + 0
                        + "\n 0 = " + 0
                        );
    }

    public void setScoreDisplay(float score, int[] accuracyTrackers)
    {
        textMesh.SetText("Score = "+score.ToString("0.00")
                        +"\n !MIKAN! = " + accuracyTrackers[10]
                        + "\n 90 = " + accuracyTrackers[9]
                        + "\n 80 = " + accuracyTrackers[8]
                        + "\n 70 = " + accuracyTrackers[7]
                        + "\n 60 = " + accuracyTrackers[6]
                        + "\n 50 = " + accuracyTrackers[5]
                        + "\n 40 = " + accuracyTrackers[4]
                        + "\n 30 = " + accuracyTrackers[3]
                        + "\n 20 = " + accuracyTrackers[2]
                        + "\n 10 = " + accuracyTrackers[1]
                        + "\n 0 = " + accuracyTrackers[0]
                        );
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
