using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    private TMP_Text textbox;
    private void Start()
    {
        textbox = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (textbox != null)
        {
            if (GameManager.IsWon)
            {
                textbox.SetText(" You Won ");
            }
            else if (GameManager.IsLost)
            {
                textbox.SetText(" You Lost ");
            }
            else
            {
                textbox.SetText("Count Down: " + ((int)GameManager.Timer));
            }
        }
    }
}
