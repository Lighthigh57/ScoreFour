using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject Pole;
    [SerializeField] private Transform board;
    [SerializeField] private Material[] materials;

    /// <summary>
    /// 盤上の情報を格納(たて、奥行き、よこ)
    /// </summary>
    private readonly int[,,] field = new int[4, 4, 4];
    /// <summary>
    /// 棒に何個入っているか(奥行き、よこ)
    /// </summary>
    private readonly int[,] onStick = new int[4, 4];

    private InterstitialAdExample interstitialAd;
    private bool stone = true;
    const int distance = 10;
    const int duration = 3;

    // Start is called before the first frame update
    void Start()
    {
        interstitialAd = GetComponent<InterstitialAdExample>();
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
                int x = (int)(vector.x + 1.5f);
                int z = (int)Mathf.Abs(vector.z - 1.5f);
                if (onStick[z, x] < 4)
                {
                    vector.y = 8;
                    GameObject game = Instantiate(sphere, vector, Quaternion.identity);
                    if (stone)
                    {
                        game.GetComponent<Renderer>().material = materials[0];
                        field[onStick[z, x]++, z, x] = 1;
                    }
                    else
                    {
                        game.GetComponent<Renderer>().material = materials[1];
                        field[onStick[z, x]++, z, x] = 2;
                    }
                    stone = !stone;
                }
                
                interstitialAd.LoadAd();
                interstitialAd.ShowAd();
                
            }
        }
        

    }
}
