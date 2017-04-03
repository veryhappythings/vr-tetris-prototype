using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public static int w = 10;
    public static int h = 20;
    public static int z = 0;
    public static float blockSize = 0.1f;
    public static Transform[,] grid = new Transform[w, h];
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject floor;

	// Use this for initialization
	void Start () {
        Vector3 p = leftWall.transform.position;
        leftWall.transform.position = new Vector3(0, p.y, p.z);
        p = rightWall.transform.position;
        rightWall.transform.position = new Vector3(w * blockSize, p.y, p.z);
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
            v.x >= 0 &&
            v.x < w * blockSize &&
            v.y >= 0
        );
    }

    public static Vector2 NearestGridPoint(Vector2 v) {
        return new Vector2(
            v.x / blockSize,
            v.y / blockSize
        );
    }

    public static Vector3 NearestGridPointPositionInSpace(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x / blockSize) * blockSize,
            Mathf.Round(v.y / blockSize) * blockSize,
            z
            );
    }
}
