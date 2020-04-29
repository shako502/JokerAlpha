using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardController : MonoBehaviour
{

    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public GameObject[] PlayerPos;

    public static string[] suits = new string[] {"clubs", "diamonds", "hearts", "spades"};
    public static Dictionary<string, int> values = new Dictionary<string, int>()
    {
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        { "10", 10 },
        { "J", 11 },
        { "Q", 12 },
        { "K", 13 },
        { "A", 14 }
    };

    public List<string>[] CardPositions;

    private List<string> cardPos1 = new List<string>();
    private List<string> cardPos2 = new List<string>();
    private List<string> cardPos3 = new List<string>();
    private List<string> cardPos4 = new List<string>();

    public List<string> deck;

    public int playedCardsNum;
    public List<GameObject> playedCardObjects = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        CardPositions = new List<string>[] {cardPos1, cardPos2, cardPos3, cardPos4};

        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        playedCardsNum = gameObject.GetComponent<UserInput>().playedCardOrder;
        
        if(playedCardsNum == 4)
        {
            cardHistory();
        }
    }

    public void cardHistory()
    {
        gameObject.GetComponent<UserInput>().playedCardOrder = 0;
        playedCardObjects = gameObject.GetComponent<UserInput>().playedCardObjects;

        foreach(GameObject playedCardObj in playedCardObjects)
        {
            Destroy(playedCardObj, 1.5f);
        }

        print("set!");
    }


    public string getPlayedCardSuit(string playedCardName)
    {
        int suitsNameStartPos = playedCardName.LastIndexOf("of") + 2;
        int suitsNameLength = playedCardName.IndexOf("-") - suitsNameStartPos;
        string playedCardSuit = playedCardName.Substring(suitsNameStartPos, suitsNameLength);
        return playedCardSuit;
    }

    public void PlayCards()
    {
        deck = GenerateDeck();
        Shuffle(deck);

        foreach(string card in deck)
        {
            //print(card);
        }

        DealPlayerCards();
        CardDeal();
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();

        foreach(string s in suits)
        {

            foreach(KeyValuePair<string, int> v in values)
            {
                if (v.Key == "6" && (s == "diamonds" || s == "spades"))
                {
                    continue;
                }
                else
                {
                    newDeck.Add($"{v.Key}of{s}-{v.Value}");
                }
            }

        }
        
        newDeck.Add("BJ-100");
        newDeck.Add("RJ-100");

        return newDeck;

    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while(n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    void CardDeal()
    {
        for(int i = 0; i < 4; i++)
        {
            float yOffset = 0;
            float xOffset = 0;

            int sortingOrder = 0;

            foreach(string card in CardPositions[i])
            {
                GameObject newCard;
                switch (i)
                {
                    case 0:
                        newCard = Instantiate(cardPrefab, new Vector3(PlayerPos[i].transform.position.x, PlayerPos[i].transform.position.y + yOffset, PlayerPos[i].transform.position.z), Quaternion.Euler(new Vector3(0, 0, -90)), PlayerPos[i].transform);
                        yOffset = yOffset + 0.5f;
                        newCard.GetComponent<Selectable>().faceUp = true;

                        newCard.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                        sortingOrder++;
                        break;
                    case 1:
                        newCard = Instantiate(cardPrefab, new Vector3(PlayerPos[i].transform.position.x, PlayerPos[i].transform.position.y + yOffset, PlayerPos[i].transform.position.z), Quaternion.Euler(new Vector3(0, 0, 90)), PlayerPos[i].transform);
                        yOffset = yOffset + 0.5f;
                        newCard.GetComponent<Selectable>().faceUp = true;

                        newCard.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                        sortingOrder++;
                        break;
                    case 2:
                        newCard = Instantiate(cardPrefab, new Vector3(PlayerPos[i].transform.position.x + xOffset, PlayerPos[i].transform.position.y, PlayerPos[i].transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)), PlayerPos[i].transform);
                        xOffset = xOffset + 0.5f;
                        newCard.GetComponent<Selectable>().faceUp = true;

                        newCard.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                        sortingOrder++;
                        break;
                    case 3:
                        newCard = Instantiate(cardPrefab, new Vector3(PlayerPos[i].transform.position.x + xOffset, PlayerPos[i].transform.position.y, PlayerPos[i].transform.position.z), Quaternion.identity, PlayerPos[i].transform);
                        xOffset = xOffset + 1;

                        newCard.GetComponent<Selectable>().faceUp = true;

                        newCard.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                        sortingOrder++;
                        break;
                    default:
                        newCard = Instantiate(cardPrefab, new Vector3(PlayerPos[i].transform.position.x + xOffset, PlayerPos[i].transform.position.y - yOffset, PlayerPos[i].transform.position.z), Quaternion.identity, PlayerPos[i].transform);
                        yOffset = yOffset + 1;
                        xOffset = xOffset + 1;
                        break;
                }
                newCard.name = card;
            }
        }
    }

    void DealPlayerCards()
    {
        
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                CardPositions[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }
            
        }
    }
}
