using UnityEngine;
using TMPro;

public class SavedAlliesCounter : MonoBehaviour
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
            textbox.SetText("Saved Allies: " + GameManager.SavedAllies + " / " + GameManager.MaxSavedAllies);
        }
    }
}
