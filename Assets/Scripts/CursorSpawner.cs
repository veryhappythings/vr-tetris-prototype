using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSpawner : MonoBehaviour {
    public Material valid;
    public Material invalid;
    public GameObject[] groups;
    private GameObject CurrentGroup;
    private GameObject WireFrame;

	// Use this for initialization
	void Start () {
        Cursor.visible = true;
        SpawnNext();
	}

    Vector3 CursorPositionInWorld () {
        Vector3 m = Input.mousePosition;
        // Translate the mouse position away from the camera down to the world
        m = new Vector3(m.x, m.y, m.z - transform.position.z);

        return GetComponent<Camera>().ScreenToWorldPoint(m);
    }

    Vector3 CursorPositionInGrid () {
        Vector3 m = Input.mousePosition;
        // Translate the mouse position away from the camera down to the world
        m = new Vector3(m.x, m.y, m.z - transform.position.z);

        Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(m);
        // Clamp the position into the grid
        Vector2 nearest = Grid.NearestGridPoint(p);
        return new Vector3(nearest.x, nearest.y, p.z);
    }

	// Update is called once per frame
	void Update () {
		if (CurrentGroup != null) {
            Vector3 p = CursorPositionInWorld();
            CurrentGroup.transform.position = new Vector3(p.x, p.y, p.z);

            Vector3 g = CursorPositionInGrid();
            WireFrame.transform.position = new Vector3(g.x, g.y, g.z);

            if (WireFrame.GetComponent<Group>().IsValidGridPosition()) {
                SetWireFrameValid();
            } else {
                SetWireFrameInvalid();
            }

            if (Input.GetMouseButtonDown(0)){
                CurrentGroup.transform.position = new Vector3(g.x, g.y, g.z);
                CurrentGroup.GetComponent<Group>().enabled = true;
                Destroy(WireFrame);
                SpawnNext();
            }

        }
	}

    void SetWireFrameValid () {
        MeshRenderer[] children = WireFrame.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in children) {
            r.material = valid;
        }
    }

    void SetWireFrameInvalid () {
        MeshRenderer[] children = WireFrame.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in children) {
            r.material = invalid;
        }
    }

    void SpawnWireFrame (int groupIndex) {
        Vector3 g = CursorPositionInGrid();
        WireFrame = Instantiate(
                groups[groupIndex],
                g,
                Quaternion.identity
        );
        SetWireFrameValid();
        WireFrame.GetComponent<Group>().enabled = false;
    }

    void SpawnNext () {
        int i = Random.Range(0, groups.Length);
        Vector3 p = CursorPositionInWorld();
        CurrentGroup = Instantiate(
                groups[i],
                p,
                Quaternion.identity
        );
        CurrentGroup.GetComponent<Group>().enabled = false;
        SpawnWireFrame(i);
    }
}
