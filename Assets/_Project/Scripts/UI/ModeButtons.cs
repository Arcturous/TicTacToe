using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ModeButtons : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons = new List<Button>();

    void Start()
    {
        foreach (Button btn in _buttons)
        {
            btn.onClick.AddListener(() =>
            {
                btn.GetComponent<Image>().color = Color.green;
                _buttons.FindAll((b) => b != btn).ForEach((btn) => btn.GetComponent<Image>().color = new Color(196, 194, 194));
            });
        }
    }
}
