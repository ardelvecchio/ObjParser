 
using UnityEngine;
using UnityEngine.UI;
using Obj;
using System.Globalization;
using System.Collections.Generic;
using TMPro;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Application = UnityEngine.Application;
using Button = UnityEngine.UI.Button;

public class Executable : MonoBehaviour {
    //these also need to be attached to a game object
    [SerializeField] InputField pathInputField;
    [SerializeField] InputField scaleInputField;
    [SerializeField] Text status;
    private List<GameObject> loaded = new List<GameObject>();
    [SerializeField] Slider slider;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button quit;
    [SerializeField] private Button menu;
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
    private void Load(string path)
    {
        Clear();
        status.text = "Loading new model...";
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        panel.gameObject.transform.localScale = new Vector3(0,0,0);
        float scale = float.Parse(scaleInputField.text, CultureInfo.InvariantCulture);

        // This line is all you need to load a model from file. Synchronous loading is also available with ObjParser.Parse()
        var model = ObjParser.Parse(path, scale);

        stopwatch.Stop();
        status.text = $"Model loaded in {stopwatch.Elapsed}";
        loaded.Add(model);
        quit.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        if (ObjParser.dataNames.Count > 0)
        {
            dropdown.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            SetDropDownMenu();
            SetModelColor();
        }
    }

    public void checkValidPath()
    {
        string path = pathInputField.text;
        
        if (path.Length == 0)
        {
            status.text = "Input Path to OBJ file";
            
        }
        else if (!File.Exists(path))
        {
            status.text = $"File does not exist";
            
        }
        else if (Path.GetExtension(path) != ".obj")
        {
            status.text = $"Not valid .obj file";
            
        }
        else
        {
            Load(path);
        }
    }
    
    public void Clear()
    {
        foreach (var model in loaded)
        {
            Destroy(model);
        }

        loaded.Clear();
    }

    public void Quit()
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

    public void bringBackScale()
    {
        panel.gameObject.transform.localScale  = new Vector3(1,1,1);
        dropdown.ClearOptions();
    }
}