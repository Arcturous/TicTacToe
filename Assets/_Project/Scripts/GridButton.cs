using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class GridButton : MonoBehaviour
{
    private Button m_button;
    private Image m_buttonImage;
    private bool m_WasClicked = false;

    void Awake()
    {
        m_buttonImage = GetComponent<Image>();
        m_button = GetComponent<Button>();

        m_button.transition = Selectable.Transition.None;

        Reset();
    }

    public void Reset()
    {
        m_WasClicked = false;

        UnlockButton();

        if (m_buttonImage)
            m_buttonImage.color = new Color(m_buttonImage.color.r, m_buttonImage.color.g, m_buttonImage.color.b, 0);
    }

    public void SetTexture(Texture2D newTexture)
    {
        if (m_button)
            m_button.interactable = false;

        m_WasClicked = true;

        if (m_buttonImage)
        {
            m_buttonImage.color = new Color(m_buttonImage.color.r, m_buttonImage.color.g, m_buttonImage.color.b, 1);
            m_buttonImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0, 0));
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