using UnityEngine;

public class Tile : MonoBehaviour
{
    public string TileID { get; set; }


    public Point GridPosition { get; private set; }


    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.z - (GetComponent<SpriteRenderer>().bounds.size.z / 2));
        }
    }


    public void Setup(Point gridPos, Vector3 worldPos)
    {
        GridPosition = gridPos;
        transform.position = worldPos;
    }


    private void PlaceObject(GameObject o)
    {
        GameObject placedObject = (GameObject)Instantiate(o, transform.position, Quaternion.identity);
        placedObject.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Z;
        placedObject.transform.SetParent(transform);
    }
}
