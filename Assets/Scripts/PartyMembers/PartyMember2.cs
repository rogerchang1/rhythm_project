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
        if (LevelManager._i.feverActive == true)
        {

            LevelManager._i.comboModifier = 2;

            if (!abilityActive)
            {
                transform.GetComponentInParent<PartyController>().showSprite(gameObject);
                abilityActive = true;
            }
        }
        else
        {
            LevelManager._i.comboModifier = 1;
            abilityActive = false;
        }
    }

}
