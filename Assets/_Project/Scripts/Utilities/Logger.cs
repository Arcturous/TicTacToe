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
        string output = _sourceClass;

        if (!string.IsNullOrEmpty(sourceFunc))
        {
            output += $"[{sourceFunc}]";
        }

        output += " " + message;

        Debug.Log(output);
    }
}