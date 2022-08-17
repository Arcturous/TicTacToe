using System;
using System.Collections.Generic;

public class GameEvent<T>
{
    // TODO make T parameter optional - right now it is required to pass it in Trigger, and into the handlers

    private List<Action<T>> _handlers = new List<Action<T>>();
    private List<Action<T>> _handlersToRemove = new List<Action<T>>();

    public void NowOn(Action<T> handler, T param)
    {
        handler(param);
        _handlers.Add(handler);
    }

    public void On(Action<T> handler)
    {
        _handlers.Add(handler);
    }

    public void Once(Action<T> handler)
    {
        _handlers.Add(handler);
        _handlersToRemove.Add(handler);
    }

    public void Off(Action<T> handler)
    {
        _handlers.Remove(handler);
        _handlersToRemove.Remove(handler);
    }

    public void Trigger(T param)
    {
        List<Action<T>> handlers = new List<Action<T>>(_handlers);
        foreach (Action<T> handler in handlers)
        {
            if (_handlersToRemove.Contains(handler))
            {
                Off(handler);
            }
            handler(param);
        }
    }
}