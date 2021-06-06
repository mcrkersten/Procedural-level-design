using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColourDatabase", menuName = "ScriptableObjects/Database/ColourDatabase", order = 0)]
public class ColourDatabase : ScriptableObject
{
    public static ColourDatabase instance;

    [SerializeField] private Color _selection;
    [SerializeField] private Color _enemySelection;
    [SerializeField] private Color _hover;
    [SerializeField] private Color _highlight;

    [SerializeField] private Material _scheduledForDemolition;
    [SerializeField] private Material _notScheduledForDemolition;

    public Color Selection { get { return _selection; } }
    public Color EnemySelection { get { return _enemySelection; } }
    public Color Hover { get { return _hover; }}
    public Color Highlight { get { return _highlight; } }

    public Material ScheduledForDemolition { get { return _scheduledForDemolition; } }
    public Material NotScheduledForDemolition { get { return _notScheduledForDemolition; } }

    public void Init()
    {
        instance = this;
    }
}
