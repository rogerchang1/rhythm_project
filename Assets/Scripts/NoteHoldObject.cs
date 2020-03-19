using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHoldObject : MonoBehaviour
{

    public Vector2 spawnPos;
    public Vector2 judgePos;
    public Vector2 removePos;
    public float beatOfThisNote;
    public float endBeatOfThisNote;
    public SongManager sm;
    public float lane;
    public bool isHit = false, pauseAtJudgeBar = false;
    public int noteType; //1 = normal, 2 = longHold, 3 = longRelease

    public GameObject HitEffect, GoodEffect, PerfectEffect, MissEffect;

    // Update is called once per frame
    void Update()
    {
        if (!pauseAtJudgeBar)
        {
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
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, .5f, 1f);
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
