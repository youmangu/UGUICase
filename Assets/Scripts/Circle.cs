using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class Circle : Image
{
    [SerializeField]
    private int segements = 100;

    [SerializeField]
    private float showPercent = 1;

    private List<Vector3> _vertexList;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        _vertexList = new List<Vector3>();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        int realSegments = (int)(segements * showPercent);

        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
        Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);

        float radian = (2 * Mathf.PI) / segements;
        float radius = width * 0.5f;


        Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
        Vector2 vertPos = Vector2.zero;

        UIVertex origin = new UIVertex();
        byte temp = (byte)(255 * showPercent);
        origin.color = new Color32(temp, temp, temp, 255);

        origin.position = originPos;
        origin.uv0 = new Vector2(vertPos.x * convertRatio.x + uvCenter.x, vertPos.y * convertRatio.y + uvCenter.y);
        vh.AddVert(origin);

        int vertexCount = realSegments + 1;
        float curRadian = 0;
        Vector2 posTemp;
        for (int i = 0; i < segements + 1; i++)
        {
            float x = Mathf.Cos(curRadian) * radius;
            float y = Mathf.Sin(curRadian) * radius;
            curRadian += radian;

            UIVertex vertexTemp = new UIVertex();
            
            if (i < vertexCount)
            {
                vertexTemp.color = color;
            }
            else
            {
                vertexTemp.color = new Color32(60, 60, 60, 255) ;
            }

            posTemp = new Vector2(x, y);
            vertexTemp.position = posTemp + originPos;
            vertexTemp.uv0 = new Vector2(posTemp.x * convertRatio.x + uvCenter.x, posTemp.y * convertRatio.y + uvCenter.y);
            vh.AddVert(vertexTemp);
            _vertexList.Add(posTemp + originPos);
        }

        int id = 1;
        for (int i = 0; i < segements; i++)
        {
            vh.AddTriangle(id, 0, id + 1);
            id++;
        }


    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);

        return IsValid(localPoint);
    }

    private bool IsValid(Vector2 localPoint)
    {
        return GetScrossPointNum(localPoint, _vertexList) % 2 == 1;
    }

    private int GetScrossPointNum(Vector2 localPoint, List<Vector3> vertexList)
    {
        Vector3 vert1 = Vector3.zero;
        Vector3 vert2 = Vector3.zero;
        int count = vertexList.Count;
        int pointCount = 0;
        for (int i = 0; i < count; i++)
        {
            vert1 = vertexList[i];
            vert2 = vertexList[(i + 1) % count];
            if (IsYInRange(localPoint, vert1, vert2))
            {
                if (localPoint.x < GetX(vert1, vert2, localPoint.y))
                {
                    pointCount++;
                }
            }
        }

        return pointCount;
    }

    private bool IsYInRange(Vector3 localPoint, Vector3 v1, Vector3 v2)
    {
        if (v1.y > v2.y)
        {
            return localPoint.y < v1.y && localPoint.y > v2.y;
        }
        else
        {
            return localPoint.y < v2.y && localPoint.y > v1.y;
        }
    }

    private float GetX(Vector3 v1, Vector3 v2, float y)
    {
        // 直线公式  y = kx + b
        float k = (v1.y - v2.y) / (v1.x - v2.x);
        return v1.x + (y - v1.y) / k;
    }
}
