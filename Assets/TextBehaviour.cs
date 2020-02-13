using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBehaviour : MonoBehaviour
{
    public GameObject plane;
    private GameObject player;
    private float boundOffset;
    private float lowXBound = -63;
    private float highXBound = 63;
    private float boxYPos = (float)191.1;
    private float boxZPos = (float)-176.8;

    // Update is called once per frame
    void Update()
    {
        boundOffset = gameObject.GetComponent<RectTransform>().rect.width / 2;

        if (player != null)
        {
            if ((player.transform.position.x > (gameObject.transform.position.x + boundOffset)) || (player.transform.position.x < (gameObject.transform.position.x - boundOffset)))
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.transform.position.x, boxYPos, boxZPos), 10f);

                if (gameObject.transform.position.x < lowXBound)
                {
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(lowXBound, boxYPos, boxZPos), 10f);

                }
                else if(gameObject.transform.position.x > highXBound)
                {
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(highXBound, boxYPos, boxZPos), 10f);

                }
            }
        }
    }

    public void setTitle(string title){

        gameObject.transform.Find("Title").GetComponent<TextMeshPro>().text = title;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

}
