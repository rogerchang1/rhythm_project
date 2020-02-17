using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    
    public LinkedList<GameObject> noteList;
    public KeyCode keyToPress;
    public SongManager sm;
    public int laneId;

    // Start is called before the first frame update
    void Start()
    {
        noteList = new LinkedList<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (noteList.Count != 0)
        {
            GameObject firstNote = noteList.First.Value;
            NoteObject2 n = firstNote.GetComponent<NoteObject2>();

            if (Input.GetKeyDown(keyToPress))
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= 2f)
                {
                    Debug.Log("Hit" + ((2f-Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats))/2f)*100f);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    noteList.RemoveFirst();
                    Destroy(firstNote);
                }
            }

            if (sm.songPosInBeats - n.beatOfThisNote > 2f && n.isHit == false)
            {
                //Instantiate(MissEffect, transform.position + new Vector3(0f,2f,0), MissEffect.transform.rotation);
                noteList.RemoveFirst();
                Destroy(firstNote);
            }
        }
    }
}
