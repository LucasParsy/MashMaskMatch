using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestLocLoad : MonoBehaviour
{
    public UnityEngine.Localization.Settings.LocalizedStringDatabase db;
    public List<string> names;

    public IEnumerator Start()
    {
        var d = db.GetTableAsync(db.DefaultTable);
        yield return d;
        var stringEntryList = d.Result.ToList();

        for (int i = 0; i < stringEntryList.Count(); i++)
        {
            var loc = stringEntryList[i].Value.GetLocalizedString();
            names.Add(loc);
        }
    }

    public string getRandomName() => names[Random.Range(0, names.Count)];

}