using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    
    public LinkedList<GameObject> noteList;
    public KeyCode keyToPress;
    public SongManager sm;
    public int laneId;
    public GameObject noteTemp; //used to temporarily keep track of long note's starting beat.
    // Start is called before the first frame update
    void Start()
    {
        noteList = new LinkedList<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float limit = sm.beatsShownInAdvance * .2f; //limit used to be just 2f, but I think that's unbalanced with the scroll speed modifier.
        if (noteList.Count != 0)
        {
            GameObject firstNote = noteList.First.Value;
            NoteObject2 n = firstNote.GetComponent<NoteObject2>();

            //regular hit
            if (Input.GetKeyDown(keyToPress) && n.noteType == 0)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                {
                    Debug.Log("Hit " + ((limit - Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats))/ limit) *100f);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    noteList.RemoveFirst();
                    Destroy(firstNote);
                }
            }

            //long hit hold
            if (Input.GetKey(keyToPress) && n.noteType == 1 && n.isHit == false)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                {
                    Debug.Log("Note Hold " + ((limit - Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats)) / limit) * 100f);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    n.isHit = true;
                    n.pauseAtJudgeBar = true;
                    noteTemp = firstNote;
                    noteList.RemoveFirst();
                }
            }
            else if (!Input.GetKey(keyToPress) && noteTemp != null)
            {
                Destroy(noteTemp);
            }

            //long hit release
            if (!Input.GetKey(keyToPress) && n.noteType == 2 && noteTemp != null)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit && noteTemp.GetComponent<NoteObject2>().isHit == true)
                {
                    Debug.Log("Note Release " + ((limit - Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats)) / limit) * 100f);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    n.isHit = true;
                    n.pauseAtJudgeBar = true;
                    noteList.RemoveFirst();
                    
                    Destroy(firstNote);
                    Destroy(noteTemp);

                }
            }
            else if (!Input.GetKey(keyToPress) && noteTemp != null)
            {
                Destroy(noteTemp);
            }

            if (sm.songPosInBeats - n.beatOfThisNote > limit && n.isHit == false)
            {
                //Instantiate(MissEffect, transform.position + new Vector3(0f,2f,0), MissEffect.transform.rotation);
                noteList.RemoveFirst();
                Destroy(firstNote);
                if(noteTemp != null)
                {
                    Destroy(noteTemp);
                }
            }
        }
    }
}
