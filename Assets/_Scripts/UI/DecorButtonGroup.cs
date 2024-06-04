using _Scripts.UI;
using UnityEngine;

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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
