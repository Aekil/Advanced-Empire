using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public struct S_TextMeshObj
    {
        // Transform params
        public Transform trParent;
        public Vector3 trLocalPos;
        // TextMesh params
        public string text;
        public int fontSize;
        public Color color;
        public TextAnchor textAnchor;
        public TextAlignment textAlignment;
        public int sortingOrder;

        public S_TextMeshObj(string x_text)
        {
            trParent = null;
            trLocalPos = Vector3.zero;
            text = x_text;
            fontSize = 20;
            color = Color.white;
            textAnchor = TextAnchor.MiddleCenter;
            textAlignment = TextAlignment.Center;
            sortingOrder = 0;
        }
        public S_TextMeshObj(string x_text, Vector3 x_localPos)
        {
            trParent = null;
            trLocalPos = x_localPos;
            text = x_text;
            fontSize = 20;
            color = Color.white;
            textAnchor = TextAnchor.MiddleCenter;
            textAlignment = TextAlignment.Center;
            sortingOrder = 0;
        }
        public S_TextMeshObj(string x_text, int x_fontSize, Color x_color)
        {
            trParent = null;
            trLocalPos = Vector3.zero;
            text = x_text;
            fontSize = x_fontSize;
            color = (x_color != null ? x_color : Color.white);
            textAnchor = TextAnchor.MiddleCenter;
            textAlignment = TextAlignment.Center;
            sortingOrder = 0;
        }
    };

    // Create text in the world
    public static TextMesh CreateWorldText(string x_text)
    {
        return CreateWorldText(new S_TextMeshObj(x_text));
    }
    public static TextMesh CreateWorldText(string x_text, Vector3 x_localPos)
    {
        return CreateWorldText(new S_TextMeshObj(x_text, x_localPos));
    }
    public static TextMesh CreateWorldText(string x_text, int x_fontSize, Color x_color)
    {
        return CreateWorldText(new S_TextMeshObj(x_text, x_fontSize, x_color));
    }
    public static TextMesh CreateWorldText(S_TextMeshObj x_textMeshObj)
    {
        GameObject gameObj = new GameObject("World_Text", typeof(TextMesh));
        gameObj.transform.SetParent(x_textMeshObj.trParent, false);
        gameObj.transform.localPosition = x_textMeshObj.trLocalPos;
        TextMesh textMesh = gameObj.GetComponent<TextMesh>();
        textMesh.anchor = x_textMeshObj.textAnchor;
        textMesh.alignment = x_textMeshObj.textAlignment;
        textMesh.text = x_textMeshObj.text;
        textMesh.fontSize = x_textMeshObj.fontSize;
        textMesh.color = x_textMeshObj.color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = x_textMeshObj.sortingOrder;
        return textMesh;
    }


    public static Vector3 GetTouchWorldPosition()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        touchPos.z = 0;
        return touchPos;
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        mousePos.z = 0f;
        return mousePos;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera x_worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, x_worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 x_screenPos, Camera x_worldCamera)
    {
        Vector3 worldPos = x_worldCamera.ScreenToWorldPoint(x_screenPos);
        return worldPos;
    }


    // Return a list of all children inside a gameObject (get all tree with recursive method)
    public static List<GameObject> GetChildren(GameObject x_gameObj)
    {
        List<GameObject> list = new List<GameObject>();
        return GetChildrenHelper(x_gameObj, list);
    }
    private static List<GameObject> GetChildrenHelper(GameObject x_gameObj, List<GameObject> x_list)
    {
        if (x_gameObj == null || x_gameObj.transform.childCount == 0)
        {
            return x_list;
        }
        foreach (Transform tr in x_gameObj.transform)
        {
            x_list.Add(tr.gameObject);
            GetChildrenHelper(tr.gameObject, x_list);
        }
        return x_list;
    }
}