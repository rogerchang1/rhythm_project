using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    
    public LinkedList<GameObject> noteList;
    public KeyCode keyToPress;
    public SongManager sm;
    public int laneId;
    public float firstNoteBeat; //debug purposes
    public GameObject noteTemp; //used to temporarily keep track of long note's starting beat.

    private float limit;

    // Start is called before the first frame update
    void Start()
    {
        noteList = new LinkedList<GameObject>();
        limit = sm.beatsShownInAdvance * .2f; //limit used to be just 2f, but I think that's unbalanced with the scroll speed modifier.
    }

    // Update is called once per frame
    void Update()
    {
        
        if (noteList.Count != 0)
        {
            GameObject firstNote = noteList.First.Value;
            NoteObject2 n = firstNote.GetComponent<NoteObject2>();
            firstNoteBeat = n.beatOfThisNote;

            //regular hit
            if (Input.GetKeyDown(keyToPress) && n.noteType == GameManager2.NOTE_NORMAL)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                {
                    noteHit(n);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    noteList.RemoveFirst();
                    Destroy(firstNote);
                }
            }

            //long hit hold
            if (Input.GetKey(keyToPress) && n.noteType == GameManager2.NOTE_HOLD && n.isHit == false)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                {
                    noteHit(n);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    n.isHit = true;
                    n.pauseAtJudgeBar = true;
                    noteTemp = firstNote;
                    noteList.RemoveFirst();
                }
            }

            //long hit release
            if (!Input.GetKey(keyToPress) && n.noteType == GameManager2.NOTE_RELEASE && noteTemp != null)
            {
                if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit && noteTemp.GetComponent<NoteObject2>().isHit == true)
                {
                    noteHit(n);
                    //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                    noteList.RemoveFirst();
                    
                    Destroy(firstNote);
                    Destroy(noteTemp);

                }
                else
                {
                    //Miss Release
                    GameManager2._i.resetCombo();
                    AccuracyPopup.Create(new Vector3(0, 1, 0), "MISS");
                    Destroy(noteTemp);
                }
            }
            

            //Note Miss
            if (sm.songPosInBeats - n.beatOfThisNote > limit && n.isHit == false)
            {
                //Instantiate(MissEffect, transform.position + new Vector3(0f,2f,0), MissEffect.transform.rotation);
                GameManager2._i.resetCombo();
                AccuracyPopup.Create(new Vector3(0, 1, 0), "MISS");
                noteList.RemoveFirst();
                Destroy(firstNote);
                if(noteTemp != null)
                {
                    Destroy(noteTemp);
                }
            }
        }
    }
    private float RoundUp(float toRound)
    {
        if (toRound % 10 == 0) return toRound;
        return (10 - toRound % 10) + toRound;
    }

    //Display accuracy on note hit and increase combo count
    private void noteHit(NoteObject2 n)
    {
        GameManager2._i.increaseCombo();
        float noteScore = RoundUp(Mathf.Ceil(((limit - Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats)) / limit) * 100f));
        string noteScoreString = "";
        if(100f - noteScore < 10)
        {
            noteScoreString = "!MIKAN!";
        }
        else if(n.beatOfThisNote < sm.songPosInBeats)
            noteScoreString = "SLOW" + noteScore.ToString();
        else
            noteScoreString = "FAST " + noteScore.ToString();

        AccuracyPopup.Create(new Vector3(0, 1, 0), noteScoreString);

    }

}
