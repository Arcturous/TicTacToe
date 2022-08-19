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

    private Vector2 m_OriginalBtnPosition;
    private Logger m_logger = new Logger("MainMenu");

    void Start()
    {
        m_OriginalBtnPosition = _modeButtons.transform.position;

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
            _modeButtons.transform.position = new Vector2(m_OriginalBtnPosition.x, m_OriginalBtnPosition.y - 0.3f);
            _difficultyButtons.SetActive(false);
        }
        else
        {
            _modeButtons.transform.position = m_OriginalBtnPosition;
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
        SceneManager.LoadScene("GameScene");
    }

    public void Reskin()
    {
        // would only show on high loading times, from my tests its too fast to see anything on local loading, but best to always have a loading message of some sort
        ShowLoadingSpinner();
        HideMenuItems();

        StartCoroutine(RequestAssetBundle(_bundleNameField.text));
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

        var assetNames = localAssetBundle.GetAllAssetNames();

        List<Texture2D> textures = new List<Texture2D>();

        foreach (string name in assetNames)
        {
            m_logger.Log($"Loading asset {name} from asset bundle");
            AssetBundleRequest assetRequest = localAssetBundle.LoadAssetAsync<Texture2D>(name);
            yield return assetRequest;

            if (assetRequest == null)
            {
                OnAssetBundleLoadError();
                yield break;
            }

            Texture2D texture = assetRequest.asset as Texture2D;

            if (texture != null)
                textures.Add(texture);
        }

        localAssetBundle.Unload(false);

        AssignTexturesToSettings(textures);
        ShowMenuItems();
        HideLoadingSpinner();
    }

    private void AssignTexturesToSettings(List<Texture2D> textures)
    {
        // BUG
        // The AssetBundle sorts them by alphabetical order, problem is I cant know the asset names beforehand.
        // I could force the asset to have x/o/bg in its name and use "find", but would be pretty bad to make external bundles meet this requirement

        _settings.textureBG = textures[0] ?? _settings.textureBG;
        _settings.textureO = textures[1] ?? _settings.textureO;
        _settings.textureX = textures[2] ?? _settings.textureX;

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
