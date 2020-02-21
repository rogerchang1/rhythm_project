using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    
    public static GameManager2 _i;
    public GameObject comboDisplay;
    public int comboTracker;
    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        comboDisplay = (GameObject)Instantiate(comboDisplay);
        comboTracker = 0;
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
        comboTracker++;
        comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboTracker.ToString());
    }


}
