using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    
    public static GameManager2 _i;
    public GameObject comboDisplay, accuracyDisplay, songObject, judgementBar;
    public int comboCount;

    public const int NOTE_NORMAL = 1;
    public const int NOTE_HOLD = 2;
    public const int NOTE_RELEASE = 3;

    public KeyCode pause;
    public bool isPause;

    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        comboDisplay = (GameObject)Instantiate(comboDisplay);
        accuracyDisplay = (GameObject)Instantiate(accuracyDisplay);
        judgementBar = (GameObject)Instantiate(judgementBar);
        songObject = (GameObject)Instantiate(songObject);
        comboCount = 0;
        isPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pause))
        {
            isPause = !isPause;
            songObject.GetComponent<SongManager>().pauseSong(isPause);
        }
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

    public void setAccuracyDisplay(string accuracy)
    {
        accuracyDisplay.GetComponent<AccuracyDisplay>().setAccuracyDisplay(accuracy);
    }

}
