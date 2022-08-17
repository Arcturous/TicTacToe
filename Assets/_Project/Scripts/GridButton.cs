using UnityEngine;
using UnityEngine.UI;
using System;

public class GridButton : MonoBehaviour
{
    private Button m_button;
    private Image m_buttonImage;
    private bool m_WasClicked = false;

    void Awake()
    {
        m_buttonImage = GetComponent<Image>();
        m_button = GetComponent<Button>();
        Reset();
    }

    public void Reset()
    {
        m_WasClicked = false;

        UnlockButton();

        if (m_buttonImage)
            m_buttonImage.color = new Color(m_buttonImage.color.r, m_buttonImage.color.g, m_buttonImage.color.b, 0);
    }

    public void SetImage(Sprite newImage)
    {
        if (m_button)
            m_button.interactable = false;

        m_WasClicked = true;

        if (m_buttonImage)
        {
            m_buttonImage.color = new Color(m_buttonImage.color.r, m_buttonImage.color.g, m_buttonImage.color.b, 1);
            m_buttonImage.sprite = newImage;
        }
    }

    public void SetOnClick(Action<int> onClick, int btnIndex)
    {
        m_button.onClick.AddListener(delegate { onClick(btnIndex); });
    }

    public void LockButton()
    {
        if (m_button)
            m_button.interactable = false;
    }

    public void UnlockButton()
    {
        if (m_button && !m_WasClicked)
            m_button.interactable = true;
    }
}