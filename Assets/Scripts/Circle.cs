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

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
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
        }

        int id = 1;
        for (int i = 0; i < segements; i++)
        {
            vh.AddTriangle(id, 0, id + 1);
            id++;
        }


    }
}
