using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public event Action OnClickedLeft, OnExit, OnClickedRight;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickedLeft?.Invoke();
        }

        if (Input.GetMouseButton(1))
        {
            OnClickedRight?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
}
