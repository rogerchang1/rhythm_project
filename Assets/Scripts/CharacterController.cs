using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private float disappearTimer;
    private Color spriteColor;
    private Color origColor;
    private SpriteRenderer spriteRender;
    private bool spriteIsActive;
    private Vector3 origPosition;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = transform.GetComponent<SpriteRenderer>();
        spriteColor = spriteRender.color;
        origColor = spriteColor;
        origPosition = transform.position;
        spriteRender.color = new Color(1f, 1f, 1f, 0f);
        spriteIsActive = false;


    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager._i.comboCounter >= 10)
        {

            LevelManager._i.characterScoreModifier = 1;
            if (!spriteIsActive)
            {
                spriteIsActive = true;
                disappearTimer = DISAPPEAR_TIMER_MAX;
                transform.position = origPosition;
                spriteColor.a = .01f;
                spriteRender.color = spriteColor;
            }
        }
        else
        {
            LevelManager._i.characterScoreModifier = 0;
            spriteIsActive = false;
        }

        if (spriteRender.color.a > 0)
        {
            
            //spriteRender.color = origColor;
            float moveXSpeed = 1f;
            transform.position += new Vector3(moveXSpeed, 0) * Time.deltaTime;

            if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
            {
                //First half of the popup lifetime
                float increaseScaleAmount = 1f;
                // transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                float decreaseScaleAmount = 1f;
                // transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                //Start disappearing
                float disappearSpeed = 3f;
                Color spriteColor = spriteRender.color;
                spriteColor.a -= disappearSpeed * Time.deltaTime;
                spriteRender.color = spriteColor;
            }
            else
            {
                float disappearSpeed = 3f;
                Color spriteColor = spriteRender.color;
                spriteColor.a += disappearSpeed * Time.deltaTime;
                spriteRender.color = spriteColor;
            }

        }

    }
}
