
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/SettingFile")]
public class GameSettings : ScriptableObject
{
    public eGameMode mode;
    public eDifficulty difficulty;
    public int gridDimension = 3;
    public int turnTime = 5;
    public Texture2D textureX;
    public Texture2D textureO;
    public Texture2D textureBG;
}