using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHoldObject : NoteObject2
{


    private void Start()
    {
        endBeatOfThisNote = beatOfThisNote;
    }
    // Update is called once per frame
    void Update()
    {
        if (!pauseAtJudgeBar)
        {
            
            if(beatOfThisNote != endBeatOfThisNote)
            {
                Vector3 tempVector = transform.localScale;
                tempVector.y = (endBeatOfThisNote - beatOfThisNote) / sm.beatsShownInAdvance;
                transform.localScale = tempVector;
            }
            else
            {
                Vector3 tempVector = transform.localScale;
                tempVector.y = 9f;
                transform.localScale = tempVector;
            }
            if (transform.position.y > judgePos.y)
            {
                transform.position = Vector2.Lerp(
                    spawnPos,
                    judgePos,
                    (sm.beatsShownInAdvance - (beatOfThisNote - sm.songPosInBeats)) / sm.beatsShownInAdvance
                );
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, .5f, 1f);
                transform.position = Vector2.Lerp(
                    judgePos,
                    removePos,
                    (sm.songPosInBeats - beatOfThisNote) / sm.beatsShownInAdvance
                );
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, judgePos.y, transform.position.z);
        }
    }
}
