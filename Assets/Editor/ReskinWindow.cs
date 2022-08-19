using System.IO;
using UnityEditor;
using UnityEngine;

public class ReskinWindow : EditorWindow
{
    private string m_bundleName = "";
    private Texture2D m_xImage;
    private Texture2D m_oImage;
    private Texture2D m_backgroundImage;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/ReskinWindow")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ReskinWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        m_bundleName = EditorGUILayout.TextField("Asset Bundle name", m_bundleName);

        m_xImage = (Texture2D)EditorGUILayout.ObjectField("X Image", m_xImage, typeof(Texture2D), false);
        m_oImage = (Texture2D)EditorGUILayout.ObjectField("O Image", m_oImage, typeof(Texture2D), false);
        m_backgroundImage = (Texture2D)EditorGUILayout.ObjectField("Background Image", m_backgroundImage, typeof(Texture2D), false);

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Build"))
        {
            if (IsBundleNameValid() && IsBundleValid())
                CreateAssetBundle();
        }
    }

    private bool IsBundleNameValid()
    {
        if (m_bundleName == "")
        {
            this.ShowNotification(new GUIContent("Bundle name is not valid"));
            return false;
        }

        string bundlePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "AssetBundles"), m_bundleName);
        if (File.Exists(bundlePath)) // should add a confirm option in the future, for now just allowing overwrite
        {
            this.ShowNotification(new GUIContent("A bundle with this name already exists, overwriting"));
            return true;
        }

        return true;
    }

    private bool IsBundleValid()
    {
        if (!m_xImage || !m_oImage || !m_backgroundImage)
        {
            this.ShowNotification(new GUIContent("Bundle is not valid"));
            return false;
        }

        return true;
    }

    private void CreateAssetBundle()
    {
        string assetPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");

        if (!Directory.Exists(assetPath))
        {
            Directory.CreateDirectory(assetPath);
        }

        AssetBundleBuild build = new AssetBundleBuild();

        build.assetBundleName = m_bundleName;
        build.assetNames = new string[]{
            AssetDatabase.GetAssetPath(m_xImage),
            AssetDatabase.GetAssetPath(m_oImage),
            AssetDatabase.GetAssetPath(m_backgroundImage)
        };

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetBundles", new AssetBundleBuild[] { build }, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }
}