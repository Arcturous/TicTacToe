using UnityEngine;

public class Logger
{
    // TODO add text coloring depending on source / on var passed in ctor
    // Debug.Log ("<color=blue>Actions completed</color>"); will color the text

    // TODO cancel logs when not debugging

    private string _sourceClass;

    public Logger(string sourceClass)
    {
        _sourceClass = $"[{sourceClass}]";
    }

    public void Log(string message, string sourceFunc = null)
    {
        Debug.Log(PrepareMessage(message, sourceFunc));
    }

    public void LogError(string message, string sourceFunc = null)
    {
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