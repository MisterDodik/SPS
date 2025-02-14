using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvases = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] private Transform parent;
    private void Awake()
    {
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }
    public T Open<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();

        return canvas;
    }
    public void Close<T>(float time) where T : UICanvas
    {
        if (IsOpened<T>())
        {
            canvases[typeof(T)].Close(time);
        }
    }
    public void CloseImmediate<T>() where T : UICanvas
    {
        if (IsOpened<T>())
        {
            canvases[typeof(T)].CloseImmediate();
        }
    }
    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvases.ContainsKey(typeof(T)) && canvases[typeof(T)] != null;
    }
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvases[typeof(T)].gameObject.activeSelf;
    }
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvases[typeof(T)] = canvas;
        }
        return canvases[typeof(T)] as T;
    }
    private T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }
    public void CloseAll()
    {
        foreach (var canvas in canvases)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
