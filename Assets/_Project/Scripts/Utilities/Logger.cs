using UnityEngine;

public class Logger
{
    // TODO add text coloring depending on source / on var passed in ctor
    // Debug.Log ("<color=blue>Actions completed</color>"); will color the text

    // TODO cancel logs when not debugging

    private string _sourceClass;

    private bool isRunningInEditor
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
        return false;
#endif
        }
    }

    public Logger(string sourceClass)
    {
        _sourceClass = $"[{sourceClass}]";
    }

    public void Log(string message, string sourceFunc = null)
    {
        if (isRunningInEditor)
            Debug.Log(PrepareMessage(message, sourceFunc));
    }

    public void LogError(string message, string sourceFunc = null)
    {
        if (isRunningInEditor)
            Debug.LogError(PrepareMessage(message, sourceFunc));
    }

    private string PrepareMessage(string message, string sourceFunc = null)
    {
        string output = _sourceClass;

        if (!string.IsNullOrEmpty(sourceFunc))
        {
            output += $"[{sourceFunc}]";
        }

        output += message;

        return output;
    }
}