using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject Pole;
    [SerializeField] private Transform board;

    private int[,,] field = new int[4, 4, 4];

    const int distance = 10;
    const int duration = 3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 vector = new(1.5f - i, 1.5f, -1.5f + j);
                GameObject obj = Instantiate(Pole, vector, Quaternion.identity);
                obj.transform.SetParent(board);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 vector = hit.transform.position;
                vector.y = 10;
                _ = Instantiate(sphere, vector, Quaternion.identity);
            }
        }

    }
}
