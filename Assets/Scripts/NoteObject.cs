using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

    public bool canBePressed;

    public KeyCode keyToPress;

    public GameObject HitEffect, GoodEffect, PerfectEffect, MissEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                //GameManager.instance.NoteHit();

                if(Mathf.Abs(transform.position.y) > 0.25)
                {
                    GameManager.instance.NormalHit();
                    Instantiate(HitEffect, transform.position, HitEffect.transform.rotation);
                    Debug.Log("Hit");
                } else if(Mathf.Abs(transform.position.y) > 0.05f)
                {
                    GameManager.instance.GoodHit();
                    Debug.Log("Good Hit");
                    Instantiate(GoodEffect, transform.position, GoodEffect.transform.rotation);
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    Debug.Log("Perfect Hit");
                    Instantiate(PerfectEffect, transform.position, PerfectEffect.transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;

            GameManager.instance.NoteMiss();
            Instantiate(MissEffect, transform.position, MissEffect.transform.rotation);

        }
    }
}
