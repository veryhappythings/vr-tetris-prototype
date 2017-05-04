using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {
    float TimeOfLastFall = 0;

	// Use this for initialization
	void Start () {
        UpdateGrid();
	}

	// Update is called once per frame
	void Update () {
        if (Time.time - TimeOfLastFall > 1) {
            transform.position += new Vector3(0, -Grid.blockSize, 0);
            if (IsValidGridPosition()) {
                UpdateGrid();
            } else {
                bool blockedByMovingBlock = BlockedByMovingBlock();
                transform.position -= new Vector3(0, -Grid.blockSize, 0);
                Debug.Log("Invalid position!");
                if (!blockedByMovingBlock)
                {
                    Debug.Log("I was blocked by a static block");
                    Grid.DeleteFullRows();
                    enabled = false;
                }
            }
            TimeOfLastFall = Time.time;
        }
	}

    void UpdateGrid () {
        // Remove self from grid
        for (int y = 0; y < Grid.h; ++y) {
            for (int x = 0; x < Grid.w; ++x) {
                if (Grid.grid[x, y] != null) {
                    if (Grid.grid[x, y].parent == transform) {
                        Grid.grid[x, y] = null;
                    }
                }
            }
        }

        // Add self to grid again
        foreach (Transform child in transform) {
            Vector2 v = Grid.NearestGridPoint(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public bool BlockedByMovingBlock() {
        foreach (Transform child in transform) {
            Vector2 v = child.position;
            Vector2 gridPoint = Grid.NearestGridPoint(v);

            if (Grid.grid[(int)gridPoint.x, (int)gridPoint.y] != null)
            {
                Transform collision = Grid.grid[(int)gridPoint.x, (int)gridPoint.y];
                if (collision.parent.GetComponent<Group>().enabled)
                {
                    if (collision.parent != transform)
                    {
                        Debug.Log("Collision with enabled block");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsValidGridPosition() {
        foreach (Transform child in transform) {
            Vector2 v = child.position;
            Vector2 gridPoint = Grid.NearestGridPoint(v);

            if (!Grid.InsideBorder(v)) {
                return false;
            }

            // If there's a block in the grid cell, it's not valid
            if (Grid.grid[(int)gridPoint.x, (int)gridPoint.y] != null)
            {
                Transform collision = Grid.grid[(int)gridPoint.x, (int)gridPoint.y];
                if (collision.parent != transform)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
