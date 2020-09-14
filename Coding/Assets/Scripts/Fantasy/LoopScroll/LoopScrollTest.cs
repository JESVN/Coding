using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
public class LoopScrollTest : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var les = _scrollRect.content.rect.size.y - _scrollRect.viewport.rect.size.y;
        if(_scrollRect.content.anchoredPosition.y>=les)
            Debug.Log($"{"到顶"}");
    }
}
