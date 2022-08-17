using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject _arrowImage;
    [SerializeField] private int _hintDisplayTime = 2;

    private Coroutine m_coroutine;

    void Start()
    {
        Hide();
    }

    public void Show(Vector2 position)
    {
        if (!_arrowImage) return;

        _arrowImage.SetActive(true);
        _arrowImage.transform.position = position;

        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }

        m_coroutine = StartCoroutine(WaitAndHideHint());
    }

    public void Hide()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }

        if (!_arrowImage) return;

        _arrowImage.SetActive(false);
    }

    private IEnumerator WaitAndHideHint()
    {
        yield return new WaitForSeconds(_hintDisplayTime);
        Hide();
    }
}