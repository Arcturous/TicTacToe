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
        _easyButton.onClick.AddListener(() =>
        {
            _easyButton.GetComponent<Image>().color = Color.green;

            _mediumButton.GetComponent<Image>().color = new Color(196, 194, 194);
            _hardButton.GetComponent<Image>().color = new Color(196, 194, 194);
        });

        _mediumButton.onClick.AddListener(() =>
        {
            _mediumButton.GetComponent<Image>().color = Color.yellow;

            _easyButton.GetComponent<Image>().color = new Color(196, 194, 194);
            _hardButton.GetComponent<Image>().color = new Color(196, 194, 194);
        });

        _hardButton.onClick.AddListener(() =>
        {
            _hardButton.GetComponent<Image>().color = Color.red;

            _mediumButton.GetComponent<Image>().color = new Color(196, 194, 194);
            _easyButton.GetComponent<Image>().color = new Color(196, 194, 194);
        });
    }
}
