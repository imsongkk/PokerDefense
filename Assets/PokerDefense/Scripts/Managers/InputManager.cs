using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(touchPos, Vector3.forward);
            RaycastHit2D[] result = Physics2D.GetRayIntersectionAll(ray);
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].collider != null)
                {
                    // var a = result[i].transform.GetComponent<Tilemap>();
                    // Debug.Log(a.GetCellCenterLocal(new Vector3Int((int)touchPos.x, (int)touchPos.y, (int)touchPos.z)));
                    //Debug.Log(result[i].collider.name);
                    //Debug.Log(result[i].transform.position);
                }
            }
        }
    }
}
