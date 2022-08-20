using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeBoard : MonoBehaviour
{

    // TODO get gridDimension from gameManager and setup the board according to screen size
    [SerializeField] private Texture2D _gridLineTexture;
    [SerializeField] private GameSettings _settings;

    private List<GridButton> m_gridButtons = new List<GridButton>();
    private List<Image> m_gridLines = new List<Image>();
    private Logger m_logger = new Logger("TicTacToeBoard");
    private int _gridDimension = 3;


    public List<GridButton> Buttons
    {
        get { return m_gridButtons; }
    }

    public int GridDimension
    {
        get { return _gridDimension; }
    }

    public void Awake()
    {
        _gridDimension = _settings?.gridDimension ?? GridDimension;

        RectTransform rectTransform = (RectTransform)gameObject.transform;

        int gridSize = (int)rectTransform.sizeDelta.x;

        float gridLeft = transform.localPosition.x - (gridSize / 2);
        float gridTop = transform.localPosition.y - (gridSize / 2);

        float lineWidth = _gridLineTexture.height / 2;  // the heught of the pic is the width of the line
        int amountOfLines = GetAmountOfLinesInGrid(GridDimension);

        float btnSize = (gridSize - (lineWidth * amountOfLines / 2)) / GridDimension;

        for (int i = 0; i < GridDimension; i++)
        {
            for (int j = 0; j < GridDimension; j++)
            {
                GameObject newBtn = new GameObject("GridButton");
                newBtn.transform.parent = transform;

                RectTransform rectTransformBtn = newBtn.AddComponent<RectTransform>();

                // set position and size
                rectTransformBtn.sizeDelta = new Vector3(btnSize, btnSize, 0);
                newBtn.transform.localScale = new Vector3(1, 1, 1);

                float posX = gridLeft + (j * btnSize) + (btnSize / 2) + (lineWidth * j);
                float posY = gridTop + (i * btnSize) + (btnSize / 2) + (lineWidth * i);
                newBtn.transform.localPosition = new Vector3(posX, posY, 0);

                // TODO see if RequireComponent completes these
                // newBtn.AddComponent<Image>();
                // newBtn.AddComponent<Button>();
                m_gridButtons.Add(newBtn.AddComponent<GridButton>());
            }
        }

        for (int i = 0; i < amountOfLines; i++)
        {
            int halfPoint = (amountOfLines - 1) / 2;
            bool isHorizontal = i > halfPoint;

            int halfPointIndex = Mathf.CeilToInt(amountOfLines / 2);

            GameObject newLine = new GameObject("GridLine" + i);
            newLine.transform.parent = transform;

            float posX;
            float posY;

            Debug.Log($"is horizontal {isHorizontal}");

            RectTransform rectTransformLine = newLine.AddComponent<RectTransform>();
            rectTransformLine.sizeDelta = new Vector3(gridSize, lineWidth, 0);
            newLine.transform.localScale = new Vector3(1, 1, 1);

            if (isHorizontal)
            {
                posX = 0;
                posY = gridTop + (btnSize * (i - halfPointIndex)) + (lineWidth / 2) + (lineWidth * (i - halfPointIndex)) + btnSize;
            }
            else
            {
                newLine.transform.rotation = Quaternion.Euler(0, 0, 90);
                posX = gridLeft + (btnSize * i) + (lineWidth / 2) + (lineWidth * i) + btnSize;
                posY = 0;
            }

            newLine.transform.localPosition = new Vector3(posX, posY, 0);

            Image lineImage = newLine.AddComponent<Image>();
            lineImage.sprite = Sprite.Create(_gridLineTexture, new Rect(0, 0, _gridLineTexture.width, _gridLineTexture.height), new Vector2(0, 0));
        }
    }

    public int GetAmountOfLinesInGrid(int gridDimension)
    {
        // 2+1 = 3  --->  X O X (turn == 2)
        // 3+1 = 4  --->  X O X O X (turn == 4)
        // 4+1 = 5  --->  X O X O X O X (turn == 6)
        // 5+1 = 6  --->  X O X O X O X O X (turn == 8)
        // 6+1 = 7  --->  X O X O X O X O X O X (turn == 10)
        // 20+1 = 21  --->  X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X O X (turn == 38)

        // GridDimension == 2 -> turn < 2
        // GridDimension == 3 -> turn < 4
        // GridDimension == 4 -> turn < 6
        // GridDimension == 5 -> turn < 8
        // GridDimension == 6 -> turn < 10
        // GridDimension == 20 -> turn < 38
        return gridDimension + 1 + (gridDimension - 3);
    }
}