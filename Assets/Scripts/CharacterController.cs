using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private const float DISAPPEAR_TIMER_MAX = .5f;
    private float disappearTimer;
    private Color spriteColor;
    private Color origColor;
    private SpriteRenderer spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = transform.GetComponent<SpriteRenderer>();
        spriteColor = spriteRender.color;
        origColor = spriteColor;
        spriteRender.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager2._i.comboCount > 50)
        {
            spriteRender.color = origColor;
            float moveXSpeed = 1f;
            transform.position += new Vector3(0, moveXSpeed) * Time.deltaTime;

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
            }
        }
    }
}
