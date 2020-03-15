using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public string songNameToLoad;
    void Start()
    {
        
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


}
