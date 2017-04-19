using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorSpawner : MonoBehaviour {
    public Material valid;
    public Material invalid;
    public GameObject[] groups;
    private GameObject CurrentGroup;
    private GameObject WireFrame;
    private SteamVR_TrackedController _controller;

	// Use this for initialization
	void Start () {
        _controller = GetComponent<SteamVR_TrackedController>();
        _controller.TriggerClicked += HandleTriggerClicked;
        Cursor.visible = true;
        SpawnNext();
	}

    void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        CurrentGroup.transform.position = WireFrame.transform.position;
        CurrentGroup.GetComponent<Group>().enabled = true;
        Destroy(WireFrame);
        SpawnNext();
    }

    Vector3 CursorPositionInWorld () {
        Vector3 m = transform.position;
        return m;
        // Translate the mouse position away from the camera down to the world
        //m = new Vector3(m.x, m.y, m.z - transform.position.z);

        //return GetComponent<Camera>().ScreenToWorldPoint(m);
    }

    Vector3 CursorPositionInGrid () {
        Vector3 p = transform.position;
        // Clamp the position into the grid
        Vector2 nearest = Grid.NearestGridPointPositionInSpace(p);
        return new Vector3(nearest.x, nearest.y, Grid.z);
    }

	// Update is called once per frame
	void Update () {
		if (CurrentGroup != null) {
            //Vector3 p = CursorPositionInWorld();
            Vector3 p = transform.position;
            CurrentGroup.transform.position = new Vector3(p.x, p.y, p.z);

            WireFrame.transform.position = Grid.NearestGridPointPositionInSpace(transform.position);
            Debug.Log(WireFrame.transform.position);

            if (WireFrame.GetComponent<Group>().IsValidGridPosition()) {
                SetWireFrameValid();
            } else {
                SetWireFrameInvalid();
            }

            if (Input.GetMouseButtonDown(0)){
                CurrentGroup.transform.position = WireFrame.transform.position;
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
        WireFrame.transform.localScale = new Vector3(Grid.blockSize, Grid.blockSize, Grid.blockSize);
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
        CurrentGroup.transform.localScale = new Vector3(Grid.blockSize, Grid.blockSize, Grid.blockSize);
        CurrentGroup.GetComponent<Group>().enabled = false;
        SpawnWireFrame(i);
    }
}
