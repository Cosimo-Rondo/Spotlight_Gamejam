using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Painting : MonoBehaviour
{
    private RectTransform canvasRectTransform;
    public Vector2 canvasSize;
    public Vector2 canvasCenter;
    public Texture2D texture;
    public Polygon borderShape;
    public bool isHiddenPainting = true;
    void Awake()
    {
        texture = GetComponentInChildren<Image>().sprite.texture;
        canvasRectTransform = GetComponent<RectTransform>();
        borderShape = new Polygon();
        UpdatePaintInfo();
        UpdatePaintAppearance();
    }

    void Update()
    {
        UpdatePaintInfo();
        UpdatePaintAppearance();
    }
    void UpdatePaintAppearance()
    {
        if (this == null) return;
        Image image = GetComponentInChildren<Image>();
        if (isHiddenPainting)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }
    void UpdatePaintInfo()
    {
        Camera mainCamera = Camera.main;
        Vector3[] corners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(corners);
        borderShape.vertices.Clear();
        borderShape.vertices.Add(corners[0]);
        borderShape.vertices.Add(corners[1]);
        borderShape.vertices.Add(corners[2]);
        borderShape.vertices.Add(corners[3]);

        // ������ռ�Ľ�λ��ת��Ϊ��Ļ�ռ�
        Vector2 bottomLeft = mainCamera.WorldToScreenPoint(corners[0]);
        Vector2 topRight = mainCamera.WorldToScreenPoint(corners[2]);

        // ����Canvas����Ļ�ռ�ĳߴ�
        canvasSize = topRight - bottomLeft;

        // ����Canvas����Ļ�ռ������λ��
        canvasCenter = (bottomLeft + topRight) / 2;
    }
}
