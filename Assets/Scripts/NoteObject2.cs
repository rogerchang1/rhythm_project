using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject2 : MonoBehaviour
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
    public float origSpriteSize;

    public GameObject HitEffect, GoodEffect, PerfectEffect, MissEffect;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = (beatOfThisNote-1f).ToString();
        origSpriteSize = transform.Find("noteholdobject").GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.y;

    }

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
                //For long hold notes, need to adjust local scale y .
                if (beatOfThisNote == endBeatOfThisNote && transform.Find("noteholdobject").gameObject.activeInHierarchy && noteType == 2)
                {
                    Transform noteHoldTransform = transform.Find("noteholdobject").transform;
                    Vector3 tempVector = noteHoldTransform.localScale;

                    tempVector.y = (spawnPos.y - transform.position.y);
                    noteHoldTransform.localScale = tempVector;
                }
                else if (beatOfThisNote != endBeatOfThisNote && transform.Find("noteholdobject").gameObject.activeInHierarchy && noteType == 2)
                {
                    Transform noteHoldTransform = transform.Find("noteholdobject").transform;
                    Vector3 tempVector = noteHoldTransform.localScale;
                    tempVector.y = (Vector2.Lerp(spawnPos, judgePos, (sm.beatsShownInAdvance - (endBeatOfThisNote - sm.songPosInBeats)) / sm.beatsShownInAdvance).y - transform.position.y);
                    noteHoldTransform.localScale = tempVector;
                }
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
            transform.position = new Vector3(transform.position.x,judgePos.y,transform.position.z);
            //For long hold notes, need to adjust local scale y .
            if (beatOfThisNote == endBeatOfThisNote && transform.Find("noteholdobject").gameObject.activeInHierarchy && noteType == 2)
            {
                Transform noteHoldTransform = transform.Find("noteholdobject").transform;
                Vector3 tempVector = noteHoldTransform.localScale;

                tempVector.y = (spawnPos.y - transform.position.y);
                noteHoldTransform.localScale = tempVector;
            }
            else if (beatOfThisNote != endBeatOfThisNote && transform.Find("noteholdobject").gameObject.activeInHierarchy && noteType == 2)
            {
                Transform noteHoldTransform = transform.Find("noteholdobject").transform;
                Vector3 tempVector = noteHoldTransform.localScale;
                tempVector.y = (Vector2.Lerp(spawnPos, judgePos, (sm.beatsShownInAdvance - (endBeatOfThisNote - sm.songPosInBeats)) / sm.beatsShownInAdvance).y - transform.position.y);
                noteHoldTransform.localScale = tempVector;
            }
        }
    }

    void ResizeSpriteToScreen(Transform t)
    {
        var sr = t.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        t.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        t.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height,0);
    }

}
