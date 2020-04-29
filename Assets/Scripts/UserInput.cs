using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private GameObject _clickedCard;
    float distanceFromCamera = 10f;
    public int playedCardOrder = 0;
    public List<GameObject> playedCardObjects = new List<GameObject>();

    public List<GameObject> ExplosiveParticles = new List<GameObject>();
    public cameraControl cameraShake;

    public GameObject JokerAnimation;
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    private void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.transform != null)
                {
                    _clickedCard = hit.transform.gameObject;
                    string Pposition = _clickedCard.transform.parent.name;

                    SpriteRenderer playedCardSprite;

                    float posX = 0.5f;
                    float posY = 0.5f;

                    switch (Pposition)
                    {
                        case "Position1":
                            posX += 0.05f;
                            break;
                        case "Position2":
                            posX -= 0.05f;
                            break;
                        case "Position3":
                            posY += 0.05f;
                            break;
                        case "Position4":
                            posY -= 0.05f;
                            break;
                    }
                    


                    //             -left,   -down
                    // new Vector3(  x,       y,      z);


                    Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(posX, posY, distanceFromCamera));
                    _clickedCard.transform.position = centerPos;

                    string playedCardSuit = gameObject.GetComponent<CardController>().getPlayedCardSuit(_clickedCard.transform.name);
                    BlowOff(playedCardOrder, posX, posY, playedCardSuit);

                    playedCardSprite = _clickedCard.GetComponent<SpriteRenderer>();
                    playedCardSprite.sortingLayerName = "PlayedCards";
                    playedCardSprite.sortingOrder = playedCardOrder;

                    playedCardObjects.Add(_clickedCard);

                    playedCardOrder += 1;
                }
                else
                {
                    return;
                }
            }
        }
    }

    void BlowOff(int order, float posX, float posY, string suit)
    {
        if (suit != "J")
        {
            print(suit);
            int suitNumber;
            switch (suit)
            {
                case "clubs":
                    suitNumber = 0;
                    break;
                case "spades":
                    suitNumber = 1;
                    break;
                case "diamonds":
                    suitNumber = 2;
                    break;
                case "hearts":
                    suitNumber = 3;
                    break;
                default:
                    suitNumber = 0;
                    break;
            }
            GameObject matchedExplosition = ExplosiveParticles[suitNumber];

            Vector3 centerPosForParticle = Camera.main.ViewportToWorldPoint(new Vector3(posX, posY, distanceFromCamera));
            GameObject generatedParticle = Instantiate(matchedExplosition, centerPosForParticle, Quaternion.identity);

            generatedParticle.GetComponent<ParticleSystemRenderer>().sortingLayerName = "PlayedCards";
            generatedParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = order;

            generatedParticle.GetComponent<ParticleSystem>().Play();

            cameraShake.triggerShake();
        }
        else
        {
            Vector3 centerPosForJoker = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCamera));
            GameObject generatedJokAnimation = Instantiate(JokerAnimation, centerPosForJoker, Quaternion.identity);

        }
    }

}
