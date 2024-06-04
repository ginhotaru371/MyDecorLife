using _Scripts.Decor;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class DecorButtonGroup : Singleton<DecorButtonGroup>
{
    public override void Awake()
    {
        base.KeepAlive(false);
        base.Awake();
    }

    private void Start()
    {
        Hide();
    }

    public void Check()
    {
        foreach (Transform child in transform.GetChild(0))
        {
            if (!child.GetComponent<DecorButton>().GetBool()) return;
        }
        
        ButtonGroup.instance.ShowCompleteButton();
    }

    public void ShowPainter()
    {
        gameObject.SetActive(true);
        foreach (Transform child in transform.GetChild(0))
        {
            if (child.GetComponent<DecorButton>().type == Type.Wall)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        foreach (Transform child in transform.GetChild(0))
        {
            if (child.GetComponent<DecorButton>().type == Type.Wall)
            {
                child.GetComponent<DecorButton>().SetBool(true);
                child.GetComponent<Button>().interactable = false;
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
