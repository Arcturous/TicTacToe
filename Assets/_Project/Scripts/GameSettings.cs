
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/SettingFile")]
public class GameSettings : ScriptableObject
{
    public eGameMode mode;
    public eDifficulty difficulty;
    public int GridDimension = 3;
}