using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBehaviour : MonoBehaviour
{
    float width = 24;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if ((player.transform.position.x > (gameObject.transform.position.x + width)) || (player.transform.position.x < (gameObject.transform.position.x - width)))
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 10f);

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
