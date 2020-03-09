using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyController : MonoBehaviour
{

    public Transform partyMemberToShow;

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private float disappearTimer;
    private Vector3 origPosition;

    // Start is called before the first frame update
    void Start()
    {
        origPosition = transform.position;
        foreach (Transform child in transform)
        {
            //"Hide" party member's sprite.
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer childSpriteRender = child.GetComponent<SpriteRenderer>();
                childSpriteRender.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (partyMemberToShow != null)
        {
            SpriteRenderer spriteRender = partyMemberToShow.GetComponent<SpriteRenderer>();
            TextMeshPro textMesh = GameObject.FindGameObjectWithTag("ModifierDisplayTag").GetComponent<TextMeshPro>();
            if (spriteRender.color.a > 0)
            {
                float moveXSpeed = 1f;
                float disappearSpeed = 3f;
                Color spriteColor = spriteRender.color;
                
                transform.position += new Vector3(moveXSpeed, 0) * Time.deltaTime;

                disappearTimer -= Time.deltaTime;
                if (disappearTimer < 0)
                {
                    //Sprite Disappearing
                    spriteColor.a -= disappearSpeed * Time.deltaTime;
                    
                }
                else
                {
                    //Sprite Appearing
                    spriteColor.a += disappearSpeed * Time.deltaTime;
                }

                spriteRender.color = spriteColor;
                textMesh.color = spriteColor;

            }
        }
    }

    public void showSprite(GameObject character)
    {
        if (partyMemberToShow != null) {
            partyMemberToShow.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0);
        }

        disappearTimer = DISAPPEAR_TIMER_MAX;
        transform.position = origPosition;
        partyMemberToShow = character.transform;
        SpriteRenderer spriteRender = partyMemberToShow.GetComponent<SpriteRenderer>();
        Color spriteColor = spriteRender.color;
        spriteColor.a = .01f;
        spriteRender.color = spriteColor;

        TextMeshPro textMesh = GameObject.FindGameObjectWithTag("ModifierDisplayTag").GetComponent<TextMeshPro>();
        textMesh.SetText(character.GetComponent<PartyMemberBase>().modifierDisplayText);
    }


}
