using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    
    public static GameManager2 _i;
    public GameObject comboDisplay;
    public int comboCount;
    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        comboDisplay = (GameObject)Instantiate(comboDisplay);
        comboCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NoteHit()
    {
    }

    public void increaseCombo()
    {
        comboCount++;
        comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboCount);
    }

    public void resetCombo()
    {
        if (comboCount > 0)
        {
            comboCount = 0;
            comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboCount);
        }
    }


}
