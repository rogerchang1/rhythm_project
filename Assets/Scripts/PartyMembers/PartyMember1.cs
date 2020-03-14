using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember1 : PartyMemberBase
{
    private int triggerValue, nextLimit;
    private bool abilityActive;
    private const int MAX_LIMIT = 250;
  
    // Start is called before the first frame update
    void Start()
    {
        triggerValue = 50;
        nextLimit = triggerValue;
        modifierDisplayText = "SCORE UP";
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager._lm.comboCounter >= triggerValue)

        {

            if (LevelManager._lm.comboCounter >= nextLimit && nextLimit <= MAX_LIMIT)
            {
                LevelManager._lm.characterScoreModifier += 1;
                nextLimit += triggerValue;
                transform.GetComponentInParent<PartyController>().showSprite(gameObject);
            }

        }
        else
        {
            LevelManager._lm.characterScoreModifier = 0;
            nextLimit = triggerValue;

        }
    }

}
