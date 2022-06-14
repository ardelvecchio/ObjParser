 
using UnityEngine;
using UnityEngine.UI;
using Obj;
using System.Globalization;
using System.Collections.Generic;
using TMPro;

public class Executable : MonoBehaviour {
    //these also need to be attached to a game object
    [SerializeField] InputField pathInputField;
    [SerializeField] InputField scaleInputField;
    [SerializeField] Text status;
    private List<GameObject> loaded = new List<GameObject>();
    [SerializeField] Slider slider;
    [SerializeField] private TMP_Dropdown dropdown;
    /*
    async public void Load()
    {
        status.text = "Loading new model...";
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        //string path = pathInputField.text;
        string path = "C:/Users/Alden/Documents/Helios/samples/radiation_selftest/build/walnut.obj";
        float scale = float.Parse(scaleInputField.text, CultureInfo.InvariantCulture);

        // This line is all you need to load a model from file. Synchronous loading is also available with ObjParser.Parse()
        var model = await ObjParser.ParseAsync(path, scale);

        stopwatch.Stop();
        status.text = $"Model loaded in {stopwatch.Elapsed}";

        if (model != null)
        {
            loaded.Add(model);
            var combinedBounds = BoundsUtils.CalculateCombinedBounds(model);
            Camera.main.transform.position = combinedBounds.center + Vector3.back * combinedBounds.size.magnitude;
        }
    }
    */
    private void Load()
    {
        status.text = "Loading new model...";
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        //string path = pathInputField.text;
        string path = "C:/Users/Alden/Documents/Helios/samples/weberpenntree_selftest/build/walnut.obj";
        //string path = "C:/Users/Alden/Documents/Helios/samples/radiation_selftest/build/walnut.obj";
        float scale = float.Parse(scaleInputField.text, CultureInfo.InvariantCulture);

        // This line is all you need to load a model from file. Synchronous loading is also available with ObjParser.Parse()
        var model = ObjParser.Parse(path, scale);

        stopwatch.Stop();
        status.text = $"Model loaded in {stopwatch.Elapsed}";
        loaded.Add(model);
        if (ObjParser.dataNames.Count > 0)
        {
            dropdown.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            SetDropDownMenu();
            SetModelColor();
        }
    }
    
    private void Clear()
    {
        foreach (var model in loaded)
        {
            Destroy(model);
        }

        loaded.Clear();
    }

    private void Quit()
    {
        Application.Quit();
    }
    
    private void ValueChangeCheck()
    {
        foreach (var model in loaded)
        {
            //make a second button, which accesses meshFilter. Let this one access MeshRenderer
            foreach (var child in model.GetComponentsInChildren<Renderer>())
            {
                for (int i = 0; i < child.sharedMaterials.Length; i++)
                {
                    child.sharedMaterials[i].SetFloat("_Gradient", slider.value);
                }
            }
            
        }
    }

    private void SetDropDownMenu()
    {
        List<string> names = ObjParser.dataNames;
        dropdown.AddOptions(names);
    }

    private void SetModelColor()
    {
        foreach (var model in loaded)
        {
            //make a second button, which accesses meshFilter. Let this one access MeshRenderer
            foreach (var child in model.GetComponentsInChildren<MeshFilter>())
            {
                
                child.mesh.colors = ObjParser.colorList[dropdown.value].ToArray();
            }
            
        }
        
    }
}