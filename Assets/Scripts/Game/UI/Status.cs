using System.Text;
using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Health Point"), Space, Range(1, 20), SerializeField]
    private int maxHealthPoint = 10;
    public int currentHealthPoint { get; private set; }

    [Space, SerializeField]
    private TextMeshProUGUI healthPointText;

    private void Awake()
    {
        SetHealthPointValue(maxHealthPoint);
    }

    private void OnValidate()
    {
        SetHealthPointValue(maxHealthPoint);
    }

    private void SetHealthPointValue(int newHealthPoint)
    {
        // Check min, max health point value
        if (newHealthPoint < 0)
        {
            newHealthPoint = 0;
        }
        else if (newHealthPoint > maxHealthPoint)
        {
            newHealthPoint = maxHealthPoint;
        }

        currentHealthPoint = newHealthPoint;

        // UI health point text
        StringBuilder text = new StringBuilder("HP ");

        text.Append('бс', currentHealthPoint);
        text.Append('бр', maxHealthPoint - currentHealthPoint);

        healthPointText.text = $"{text}";
    }
}
