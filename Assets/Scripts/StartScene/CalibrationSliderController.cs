using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalibrationSliderController : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshPro textMesh;
    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        if (GameManager.calibration == 0)
            textMesh.SetText("Calibration : 0");
        else
            textMesh.SetText("Calibration : " + GameManager.calibration.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(float c)
    {
        textMesh.SetText("Calibration : " + (Mathf.Round(c * 10f) / 10f).ToString());
    }
}
