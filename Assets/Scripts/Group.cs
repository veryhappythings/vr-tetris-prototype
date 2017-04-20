using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {
    float TimeOfLastFall = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        if (Time.time - TimeOfLastFall > 1) {
            transform.position += new Vector3(0, -Grid.blockSize, 0);
            if (IsValidGridPosition()) {
                UpdateGrid();
            } else {
                transform.position -= new Vector3(0, -Grid.blockSize, 0);
                Grid.DeleteFullRows();
                //FindObjectOfType<Spawner>().SpawnNext();
                enabled = false;
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
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public bool IsValidGridPosition() {
        foreach (Transform child in transform) {
            Vector2 v = child.position;
            Vector2 gridPoint = Grid.NearestGridPoint(v);
            Debug.Log("Pos: " + v);
            Debug.Log("Grid point: " + gridPoint);

            if (!Grid.InsideBorder(v)) {
                Debug.Log("Not in border");
                return false;
            }

            // If there's a block in the grid cell, it's not valid
            if (Grid.grid[(int)gridPoint.x, (int)gridPoint.y] != null && Grid.grid[(int)gridPoint.x, (int)gridPoint.y].parent != transform) {
                return false;
            }
        }

        return true;
    }
}
