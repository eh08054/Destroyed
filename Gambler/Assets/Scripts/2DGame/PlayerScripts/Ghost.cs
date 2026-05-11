using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : MonoBehaviour
{

    public float ghostDelay;
    private float ghostDelayTime;
    public GameObject ghost;
    public bool makeGhost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ghostDelayTime = ghostDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (makeGhost)
        {
            if(ghostDelayTime > 0)
            {
                ghostDelayTime -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite CurrentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = CurrentSprite;
                ghostDelayTime = ghostDelay;
                Destroy(currentGhost, 0.4f);
            }
        }
    }
}
