using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelController : MonoBehaviour
{
    public string songNameToLoad;
    void Start()
    {
        gameObject.GetComponentInChildren<TextMeshPro>().SetText(songNameToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel()
    {
        GameManager._gm.songNameToLoad = songNameToLoad;
        GameManager._gm.LoadLevel();
    }

    void OnMouseDown()
    {
        loadLevel();
    }

    private void bounce()
    {

    }

}
