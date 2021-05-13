using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDiagram2D : MonoBehaviour
{
    public Vector2 ItemSize;
    public Sprite[] ItemSprites;
    public float ScaleTimesMin;
    public float ScaleTimesMax;

    private List<RotationDiagramItem> _items;
    private List<ItemPosData> _posDAtas;

    void Start()
    {
        _items = new List<RotationDiagramItem>();
        _posDAtas = new List<ItemPosData>();
        CreateItem();
    }

    private GameObject CreateTemplate()
    {
        GameObject item = new GameObject("Template");
        item.AddComponent<RectTransform>().sizeDelta = ItemSize;
        item.AddComponent<Image>();
        item.AddComponent<RotationDiagramItem>();

        return item;
    }

    private void CreateItem()
    {
        GameObject template = CreateTemplate();
        RotationDiagramItem itemTemp = null;
        foreach (Sprite sprite in ItemSprites)
        {
            itemTemp = Instantiate(template).GetComponent<RotationDiagramItem>();
            itemTemp.SetParent(transform);
            _items.Add(itemTemp);
            itemTemp.SetSprite(sprite);
        }

        Destroy(template);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radio">比例</param>
    /// <param name="length"></param>
    /// <returns></returns>
    private float GetX(float radio, float length)
    {
        if (radio > 1 || radio < 0)
        {
            Debug.LogError("当前比例必须是0-1的值");
            return 0;
        }

        if (radio > 0 && radio < 0.25f)
        {
            return length * radio;
        }
        else if (radio >= 0.5f && radio < 0.75f)
        {
            return length * (0.5f - radio);
        }
        else
        {
            return length * (radio - 1);
        }
    }
}


public struct ItemPosData
{
    public int X;
    public float ScaleTimes;
}
