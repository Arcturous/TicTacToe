using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;
    [SerializeField] private GameObject _modeButtons;
    [SerializeField] private GameObject _difficultyButtons;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Texture2D _defaultX;
    [SerializeField] private Texture2D _defaultO;
    [SerializeField] private Texture2D _defaultBG;
    [SerializeField] private List<GameObject> _menuItems;
    [SerializeField] private Image _loadingSpinner;
    [SerializeField] private InputField _bundleNameField;
    [SerializeField] private InputField _gridSizeField;
    [SerializeField] private Text _gridSizeText;

    private Vector2 m_originalBtnPosition;
    private Logger m_logger = new Logger("MainMenu");

    void Start()
    {
        m_originalBtnPosition = _modeButtons.transform.position;

        // on first run - use defaults
        if (!_settings.textureX)
        {
            _settings.gridDimension = 3;
            _settings.textureX = _defaultX;
            _settings.textureO = _defaultO;
            _settings.textureBG = _defaultBG;
        }

        // always reset mode
        _settings.mode = eGameMode.PvPC;
        _settings.difficulty = eDifficulty.easy;

        _backgroundImage.sprite = Sprite.Create(_settings.textureBG, new Rect(0, 0, _settings.textureBG.width, _settings.textureBG.height), new Vector2(0, 0));

        _gridSizeField.onValueChanged.AddListener((newVal) =>
        {
            if (CheckAndSetGridDimension())
            {
                _gridSizeText.color = Color.black;
            }
        });
    }

    void Update()
    {
        RotateSpinner();
    }

    private void RotateSpinner()
    {
        if (!_loadingSpinner.enabled) return;

        _loadingSpinner.transform.Rotate(0f, 0f, -500f * Time.deltaTime);
    }

    public void SetGameMode(int mode)
    {
        _settings.mode = (eGameMode)mode;

        m_logger.Log($"new mode {_settings.mode}");

        if (_settings.mode == eGameMode.PvP)
        {
            _modeButtons.transform.position = new Vector2(m_originalBtnPosition.x, m_originalBtnPosition.y - 0.3f);
            _difficultyButtons.SetActive(false);
        }
        else
        {
            _modeButtons.transform.position = m_originalBtnPosition;
            _difficultyButtons.SetActive(true);
        }
    }

    public void SetDifficulty(int difficulty)
    {
        _settings.difficulty = (eDifficulty)difficulty;
        m_logger.Log($"new difficulty {_settings.difficulty}");
    }

    public void SetGridDimension(int dimension)
    {
        _settings.gridDimension = dimension;
    }

    public void StartGame()
    {
        if (!CheckAndSetGridDimension())
        {
            _gridSizeText.color = Color.red;
            if (!_gridSizeField.text.Contains("(invalid, need number 3-10)"))
                _gridSizeField.text = _gridSizeField.text + "(invalid, need number 3-10)";
            return;
        }
        SceneManager.LoadScene("GameScene");
    }

    public void Reskin()
    {
        // would only show on high loading times, from my tests its too fast to see anything on local loading, but best to always have a loading message of some sort
        ShowLoadingSpinner();
        HideMenuItems();

        StartCoroutine(RequestAssetBundle(_bundleNameField.text));
    }

    private bool CheckAndSetGridDimension()
    {
        if (!_gridSizeField && _gridSizeField.text == null && _gridSizeField.text == "")
        {
            return false;
        }

        int gridDimension;
        if (int.TryParse(_gridSizeField.text, out gridDimension))
        {
            if (gridDimension > 10 || gridDimension < 3)
                return false;

            _settings.gridDimension = gridDimension;
        }
        else
        {
            return false;
        }

        return true;
    }

    private IEnumerator RequestAssetBundle(string bundleName)
    {
        if (bundleName == null || bundleName == "")
        {
            OnAssetBundleLoadError();
            yield break;
        }

        string bundlePath = Path.Combine(Application.streamingAssetsPath + "/AssetBundles", bundleName);

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundle localAssetBundle = request.assetBundle;
        if (localAssetBundle == null)
        {
            OnAssetBundleLoadError();
            yield break;
        }

        AssetBundleRequest assetRequest = localAssetBundle.LoadAllAssetsAsync<Texture2D>();
        yield return assetRequest;

        if (assetRequest == null)
        {
            OnAssetBundleLoadError();
            yield break;
        }

        object[] textures = assetRequest.allAssets;
        // m_logger.Log(string.Join(", ", textures as object[]));

        localAssetBundle.Unload(false);

        AssignTexturesToSettings(textures);
        ShowMenuItems();
        HideLoadingSpinner();
    }

    private void AssignTexturesToSettings(object[] textures)
    {
        if (textures == null)
        {
            OnAssetBundleLoadError();
            return;
        }

        // The assets will always be ordered like this - I rename the texture files, and then rename them back to the original name - this causes them to always be in the same order as the AssetBundle orders them alphabetically
        _settings.textureBG = textures[0] as Texture2D ?? _settings?.textureBG;
        _settings.textureO = textures[1] as Texture2D ?? _settings?.textureO;
        _settings.textureX = textures[2] as Texture2D ?? _settings?.textureX;

        _backgroundImage.sprite = Sprite.Create(_settings.textureBG, new Rect(0, 0, _settings.textureBG.width, _settings.textureBG.height), new Vector2(0, 0));
    }

    private void OnAssetBundleLoadError()
    {
        m_logger.LogError("Failed to load Asset Bundle");
        ShowMenuItems();
        HideLoadingSpinner();
    }

    private void ShowMenuItems()
    {
        _menuItems.ForEach((item) => item.SetActive(true));
    }

    private void HideMenuItems()
    {
        _menuItems.ForEach((item) => item.SetActive(false));
    }

    private void ShowLoadingSpinner()
    {
        _loadingSpinner.enabled = true;
    }

    private void HideLoadingSpinner()
    {
        _loadingSpinner.enabled = false;
    }
}
