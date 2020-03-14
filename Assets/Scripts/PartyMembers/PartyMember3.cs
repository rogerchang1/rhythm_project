using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember3 : PartyMemberBase
{
    private bool abilityActive, inActive;
    private const int MAX_ACTIVATIONS = 2;
    private const int MAX_HITS = 50;
    private int activationCounter, noteCounterEnd;
    // Start is called before the first frame update
    void Start()
    {
        activationCounter = 0;
        inActive = false;
        noteCounterEnd = 0;
        modifierDisplayText = "HEAL UP";
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager._lm.currentHealth <= 50 && activationCounter <= MAX_ACTIVATIONS && !inActive)
        {
            if (!inActive)
            {
                inActive = true;
                activationCounter++;
                LevelManager._lm.healModifier = 1;
                noteCounterEnd = LevelManager._lm.noteCounter + 40;
            }
            

            if (!abilityActive)
            {
                transform.GetComponentInParent<PartyController>().showSprite(gameObject);
                abilityActive = true;
            }
        }
        if(inActive && LevelManager._lm.noteCounter > noteCounterEnd)
        {
            LevelManager._lm.healModifier = 0;
            abilityActive = false;
            inActive = false;
        }
    }

}
