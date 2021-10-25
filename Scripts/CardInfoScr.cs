using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoScr : MonoBehaviour
{
    public CardController CardController;

    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defence, Manacost;
    public GameObject hideObj, HighlitedObj;
    public Color NormalCol, TargetCol;
    public void HideCardInfo()
    {
        hideObj.SetActive(true);
        Manacost.text = "";
    }



    public void ShowCardInfo()
    {

        hideObj.SetActive(false);
        Logo.sprite = CardController.Card.Logo;
        Logo.preserveAspect = true;
        Name.text = CardController.Card.Name;

        RefreshData();
    }

    public void RefreshData()
    {
        Attack.text = CardController.Card.Attack.ToString();
        Defence.text = CardController.Card.Defence.ToString();
        Manacost.text = CardController.Card.Manacost.ToString();
    }
  
    public void HighlightCard(bool highlight)
    {
        HighlitedObj.SetActive(highlight);
    }

    

    public void HighLightManaAvaliability(int currentMana)
    {
        GetComponent<CanvasGroup>().alpha = currentMana >= CardController.Card.Manacost ?
                    1 : .5f;
    }

    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }
}
