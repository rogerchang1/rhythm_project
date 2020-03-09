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
        if (LevelManager._i.comboCounter >= triggerValue)

        {

            if (LevelManager._i.comboCounter >= nextLimit && nextLimit <= MAX_LIMIT)
            {
                LevelManager._i.characterScoreModifier += 1;
                nextLimit += triggerValue;
                transform.GetComponentInParent<PartyController>().showSprite(gameObject);
            }

        }
        else
        {
            LevelManager._i.characterScoreModifier = 0;
            nextLimit = triggerValue;

        }
    }

}
