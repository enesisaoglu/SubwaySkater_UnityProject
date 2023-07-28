using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    public static SaveManager instance;



    // Fields
    public SaveState save;
    private const string SAVEFILENAME = "data.ss";
    private BinaryFormatter formatter;

    // Actions...
    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

    private void Awake()
    {
        instance = this;
        
        formatter = new BinaryFormatter();

        // Try and load the previous save state...
        Load();
    }

    private void Load()
    {
        try
        {
            // This will return us a file stream to store in file...
            FileStream file = new FileStream(SAVEFILENAME, FileMode.Open, FileAccess.Read);
            save = formatter.Deserialize(file) as SaveState;// Deserialize...
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found, let's create a new one!");
            Save();
        }
       
    }
        
    public void Save()
    {
        // If there is no previous state found, create a new one!
        if (save == null)
        {
            save = new SaveState();
        }

        // Set the time at which we have tried saving...
        save.LastSaveTime = DateTime.Now;

        // Open a file on our system, and write to it...
        FileStream file = new FileStream(SAVEFILENAME, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);
    }
}
