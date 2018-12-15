using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevelButton : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Button btn;

    internal void SetLevel(DLevel level, Action onClick)
    {
        txt.text = level.id;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            onClick();
        });
    }
}
