using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public enum CardType { Attack, Skill, Power, Curse, Status};
public enum CardPlace { Hand, Deck, Exhaust, Discard };
public class Card : MonoBehaviour
{
    public int manaCost;
    public IntVariable playerMana;
    public string title;
    public CardType type;
    public string description;
    public TextMeshPro manaCostText;
    public TextMeshPro titleText;
    public TextMeshPro typeText;
    public TextMeshPro descriptionText;

    public CardPositioningVariable cardPositioning;
    
    public CardPlace myPlace;
    public GameObjectGameEvent discardCardEvent;
    public GameObjectGameEvent cardPlayedEvent;

    private void Start()
    {
        SetTextOnCard();
    }

    public void WhenIAmDrawn(GameObject whichCardWasDrawn)
    {
        if (whichCardWasDrawn == this.gameObject)
        {
            myPlace = CardPlace.Hand;
        }
        MoveToTheCorrectPositionInHand();
    }

    public void WhenIAmDiscarded(GameObject whichCardWasDiscarded)
    {
        if (whichCardWasDiscarded == this.gameObject)
        {
            myPlace = CardPlace.Discard;
        }
        MoveToTheCorrectPositionInHand();
    }

    public void WhenIAmPutIntoTheDeck(GameObject whichCardWasPutIntoTheDeck)
    {
        if (whichCardWasPutIntoTheDeck == this.gameObject)
        {
            myPlace = CardPlace.Deck;
        }
    }

    public void WhenIAmExhausted(GameObject whichCardWasExhausted)
    {
        if (whichCardWasExhausted == this.gameObject)
        {
            myPlace = CardPlace.Exhaust;
        }
    }


    public void MoveToTheCorrectPositionInHand()
    {
        if (myPlace == CardPlace.Hand)
        {
            int currentPositionIndex = transform.GetSiblingIndex();
            int siblingCount = transform.parent.childCount;

            Vector3 startPosition = new Vector3(cardPositioning.Value.xPositionRange.x - (cardPositioning.Value.xPositionIncreasePerCard * siblingCount), cardPositioning.Value.yPosition, 0);
            Vector3 endPosition = new Vector3(cardPositioning.Value.xPositionRange.y + (cardPositioning.Value.xPositionIncreasePerCard * siblingCount), cardPositioning.Value.yPosition, 0);
            if (siblingCount - 1.0f != 0.0f)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, (float)currentPositionIndex / ((float)siblingCount - 1.0f));
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, 0.5f);
            }
        }
    }

    public void SetTextOnCard()
    {
        //Go to the mana, title, type, description and set them to the values of what the card should be

        manaCostText.text = manaCost.ToString();
        titleText.text = title;
        typeText.text = type.ToString();
        descriptionText.text = description;
    }

    public virtual void OnMouseDown()
    {
    }

    public virtual void OnMouseUp()
    {
        
    }
}
