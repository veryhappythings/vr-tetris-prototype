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
        Vector3 leftWallPosition = leftWall.transform.position;
        leftWall.transform.position = new Vector3(0 - (blockSize / 2), leftWallPosition.y, leftWallPosition.z);
        Vector3 rightWallPosition = rightWall.transform.position;
        rightWall.transform.position = new Vector3(((w - 1) * blockSize) + (blockSize / 2), rightWallPosition.y, rightWallPosition.z);
	}

	// Update is called once per frame
	void Update () {

	}

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            DecreaseRow(i);
    }

    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    public static void DeleteFullRows () {
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }

    public static Vector2 RoundVec2(Vector2 v) {
        return new Vector2(
                Mathf.Round(v.x),
                Mathf.Round(v.y)
        );
    }

    public static bool InsideBorder(Vector2 v) {
        float rightEdge = w * blockSize;
        return (
            v.x >= 0 &&
            v.x < rightEdge &&
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
