using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DifficultyButtons : MonoBehaviour
{
    [SerializeField] Button _easyButton;
    [SerializeField] Button _mediumButton;
    [SerializeField] Button _hardButton;

    void Start()
    {
        Color disabledColor = new Color(196, 194, 194);

        Image easyImage = _easyButton.GetComponent<Image>();
        Image mediumImage = _mediumButton.GetComponent<Image>();
        Image hardImage = _hardButton.GetComponent<Image>();

        _easyButton.onClick.AddListener(() =>
        {
            easyImage.color = Color.green;

            mediumImage.color = disabledColor;
            hardImage.color = disabledColor;
        });

        _mediumButton.onClick.AddListener(() =>
        {
            mediumImage.color = Color.yellow;

            easyImage.color = disabledColor;
            hardImage.color = disabledColor;
        });

        _hardButton.onClick.AddListener(() =>
        {
            hardImage.color = Color.red;

            mediumImage.color = disabledColor;
            easyImage.color = disabledColor;
        });
    }
}
