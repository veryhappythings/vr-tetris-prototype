using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public static int w = 10;
    public static int h = 20;
    public static Transform[,] grid = new Transform[w, h];

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public static void DeleteFullRows () {

    }

    public static Vector2 RoundVec2(Vector2 v) {
        return new Vector2(
                Mathf.Round(v.x),
                Mathf.Round(v.y)
        );
    }

    public static bool InsideBorder(Vector2 v) {
        return (
            (int)v.x >= 0 &&
            (int)v.x < w &&
            (int)v.y >= 0
        );
    }

    public static Vector2 NearestGridPoint(Vector2 v) {
        return new Vector2(
            Mathf.Round(Mathf.Clamp(v.x, 0, w - 1)),
            Mathf.Round(Mathf.Clamp(v.y, 0, h - 1))
        );
    }
}
