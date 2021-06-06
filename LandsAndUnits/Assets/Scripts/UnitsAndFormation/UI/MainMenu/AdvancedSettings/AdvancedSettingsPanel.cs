using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedSettingsPanel : MonoBehaviour
{
    [SerializeField] private FoilageLayerSettings _foilageSettings;
    [SerializeField] private FoilageGenerator _generator;
    public GameObject _foilageLayerPanelPrefab;
    public Button _reGenerateButton;
    private List<GameObject> _instantiated = new List<GameObject>();

    public void Start()
    {
        CreateAllLayerSettings();
        _reGenerateButton.onClick.AddListener(Regenerate);
    }

    private void CreateAllLayerSettings()
    {
        foreach (FoilageLayer layer in _foilageSettings._foilageLayers)
        {
            if (layer._visableInGameMenu)
            {
                GameObject x = Instantiate(_foilageLayerPanelPrefab, this.transform);
                FoilageLayerMenuPanel panel = x.GetComponent<FoilageLayerMenuPanel>();
                panel._title.text = layer._type.ToString();
                _instantiated.Add(x);
                foreach (ResourceSpawnOptions option in layer._resourceSpawnOptions)
                {
                    GameObject xx = Instantiate(panel._foilageLayerMenuObjectPrefab, x.transform);
                    FoilageLayerMenuObject foilageObject = xx.GetComponent<FoilageLayerMenuObject>();
                    foilageObject._layer = option;
                    foilageObject._typeTitle.text = option._type.ToString();
                    foilageObject._maxInput.text = option._maximum.ToString();
                    foilageObject._minInput.text = option._minimum.ToString();
                }
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    private void Regenerate()
    {
        _generator.InstantiateAllObjects();
    }
}
