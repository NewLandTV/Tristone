using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("World Settings"), Space, SerializeField]
    private World world;
    [SerializeField]
    private Transform character;

    [Space, Header("UI Settings"), Space, SerializeField]
    private Talk talk;

    [Space, Header("Tutorial Settings"), Space, SerializeField]
    private TalkData tutorialTalkData;
    [SerializeField]
    private GameObject[] gameObjects;

    [Space, Header("NPC Settings"), Space, SerializeField]
    private NPC npcPrefab;
    [Range(0, 100), SerializeField]
    private int spawnCount;

    // Called when the world view timeline ends.
    public void WorldViewEnd()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
        }

        world.Target = character;

        talk.StartTalk(tutorialTalkData, CompleteTutorial);
    }

    private void CompleteTutorial()
    {
        SpawnNPC();
    }

    private void SpawnNPC()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // The spawn area is 16x16
            float x = Random.Range(-8f, 8f);
            float z = Random.Range(-8f, 8f);

            if (x == 0f)
            {
                x = Random.Range(0, 2) == 0 ? 1f : -1f;
            }

            if (z == 0f)
            {
                z = Random.Range(0, 2) == 0 ? 1f : -1f;
            }

            Vector3 offset = new Vector3(x, 0f, z);

            Instantiate(npcPrefab, transform.position + offset, Quaternion.identity);
        }
    }
}
