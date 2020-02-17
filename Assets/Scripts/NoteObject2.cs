﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject2 : MonoBehaviour
{
    public Vector2 spawnPos;
    public Vector2 judgePos;
    public Vector2 removePos;
    public float beatOfThisNote;
    public SongManager sm;
    public float lane;
    public bool isHit = false;

    public GameObject HitEffect, GoodEffect, PerfectEffect, MissEffect;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = (beatOfThisNote-1f).ToString();
    }

    // Update is called once per frame
    void Update()
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
}
