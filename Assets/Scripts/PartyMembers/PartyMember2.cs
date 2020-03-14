using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember2 : PartyMemberBase
{
    private bool abilityActive;

    // Start is called before the first frame update
    void Start()
    {
        modifierDisplayText = "COMBO UP";
        abilityActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager._lm.feverActive == true)
        {

            LevelManager._lm.comboModifier = 2;

            if (!abilityActive)
            {
                transform.GetComponentInParent<PartyController>().showSprite(gameObject);
                abilityActive = true;
            }
        }
        else
        {
            LevelManager._lm.comboModifier = 1;
            abilityActive = false;
        }
    }

}
