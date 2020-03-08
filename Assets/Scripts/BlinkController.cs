using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkController : MonoBehaviour
{
    private float DISAPPEAR_TIMER = 1f;
    private float APPEAR_TIMER = 2.5f;
    private TextMeshPro textMesh;
    public float secPerBeat;
    public float dsptimestart;
    public float dsptime;
    public float audioPos;
    public bool isBlinking;
    public float a;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinking)
        {
            Color textMeshColor = textMesh.color;
            dsptime = (float)AudioSettings.dspTime;
            if ((float)AudioSettings.dspTime - dsptimestart > audioPos + secPerBeat)
            {
                audioPos += secPerBeat;
                textMeshColor.a = 1;
            }
            else
            {
                textMeshColor.a -= DISAPPEAR_TIMER * Time.deltaTime;
            }
            a = textMeshColor.a;
            /*
            if (textMeshColor.a <= 0 && fade)
                fade = !fade;
            if (textMeshColor.a >= 1 && !fade)
                fade = !fade;

            if(fade)
                textMeshColor.a -= DISAPPEAR_TIMER * Time.deltaTime;
            else
                textMeshColor.a += APPEAR_TIMER * Time.deltaTime;
            */

            textMesh.color = textMeshColor;
        }
    }

    public void setTempo(float bpm)
    {
        secPerBeat = 60f / bpm;
        dsptimestart = (float)AudioSettings.dspTime;
        audioPos = 0;
        isBlinking = true;

    }
}
