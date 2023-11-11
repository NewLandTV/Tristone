using System.Collections.Generic;
using UnityEngine;

public class World : MovingObject
{
    [Header("General Settings"), Space, SerializeField]
    private new string name;
    [SerializeField]
    private bool useWorldBorder = true;

    [Space, Header("Scale Settings"), Space, SerializeField]
    private int width = 256;
    [SerializeField]
    private int depth = 256;

    [Space, Header("Rendering Settings"), Space, SerializeField]
    private Transform target;
    public Transform Target { get => target; set => target = value; }
    [Range(1, 48), SerializeField]
    private int renderBoxRange = 8;
    [Range(1, 48), SerializeField]
    private int renderBoxGrassRange = 4;

    [Space, Header("Box Settings"), Space, SerializeField]
    private Box boxPrefab;
    [SerializeField]
    private BoxCollider groundCollider;

    [Space, Range(1, 32), SerializeField]
    private int boxSize = 4;

    // World boxes
    private List<GameObject> renderBoxes = new List<GameObject>();
    private Box[,] boxes;

    // Rendering optimization
    private Vector3 previousCharacterPosition;

    private void Awake()
    {
        Setup();
        ApplyNewSize();
        RenderBoxesNearCharacter();

        if (useWorldBorder)
        {
            MakeWorldBorder();
        }
    }

    private void OnValidate()
    {
        Setup();
        ApplyNewSize();
    }

    private void Update()
    {
        if ((previousCharacterPosition - target.position).sqrMagnitude > boxSize)
        {
            RenderBoxesNearCharacter();
        }
    }

    private void ApplyNewSize()
    {
        boxes = new Box[width, depth];

        transform.position = new Vector3(boxSize * 0.5f, 0f, boxSize * 0.5f);
        groundCollider.transform.localPosition = new Vector3(-boxSize * 0.5f, -0.5f, -boxSize * 0.5f);
        groundCollider.size = new Vector3(width * boxSize, 0.1f, depth * boxSize);
    }

    private void MakeBox(int x, int z)
    {
        Vector3 position = new Vector3((x - (width >> 1)) * boxSize, -0.5f, (z - (depth >> 1)) * boxSize);
        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f);

        boxes[x, z] = Instantiate(boxPrefab, Vector3.zero, rotation, transform);

        boxes[x, z].transform.localPosition = position;
        boxes[x, z].transform.localScale = new Vector3(boxSize * 0.1f, 1f, boxSize * 0.1f);
    }

    /// <summary>
    /// Make boxes near the character and activates boxes only near the character.
    /// </summary>
    private void RenderBoxesNearCharacter()
    {
        previousCharacterPosition = target.position;

        int offsetX = width >> 1;
        int offsetZ = depth >> 1;
        int characterX = Mathf.RoundToInt(target.position.x / boxSize) + offsetX;
        int characterZ = Mathf.RoundToInt(target.position.z / boxSize) + offsetZ;

        // Disable previously rendered boxes
        for (int i = 0; i < renderBoxes.Count; i++)
        {
            renderBoxes[i].SetActive(false);
        }

        renderBoxes.Clear();

        for (int x = characterX - renderBoxRange; x < characterX + renderBoxRange; x++)
        {
            for (int z = characterZ - renderBoxRange; z < characterZ + renderBoxRange; z++)
            {
                // If out of indexs, return from here : Index Out Of Range check
                if (x < 0 || x >= width || z < 0 || z >= depth)
                {
                    continue;
                }

                if (boxes[x, z] == null)
                {
                    MakeBox(x, z);
                }

                renderBoxes.Add(boxes[x, z].gameObject);

                boxes[x, z].gameObject.SetActive(true);

                bool activeGrass = x >= characterX - renderBoxGrassRange && x < characterX + renderBoxGrassRange && z >= characterZ - renderBoxGrassRange && z < characterZ + renderBoxGrassRange;

                boxes[x, z].SetActiveGrass(activeGrass);
            }
        }
    }

    private void MakeWorldBorder()
    {

    }
}
