using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

namespace Codari.TTT
{
    //public sealed class TTTProfileManager : MonoBehaviour, IReadOnlyList<TTTProfile>
    //{
    //    private const string ProfileListKey = "ttt_profiles";

    //    [ReadOnly]
    //    [SerializeField]
    //    private List<TTTProfile> profileList = new List<TTTProfile>();

    //    private Dictionary<string, TTTProfile> profileLookup = new Dictionary<string, TTTProfile>(StringComparer.OrdinalIgnoreCase);

    //    public int ProfileCount => profileList.Count;

    //    public TTTProfile this[int index] => profileList[index];

    //    public TTTProfile this[string name] => profileLookup[name];

    //    public bool ProfileExistsWithName(string name) => profileLookup.ContainsKey(name);

    //    public int IndexOfProfile(TTTProfile profile) => profileList.IndexOf(profile);

    //    public int IndexOfProfile(string name)
    //    {
    //        TTTProfile profile;
    //        if (profileLookup.TryGetValue(name, out profile))
    //        {
    //            return IndexOfProfile(profile);
    //        }

    //        return -1;
    //    }

    //    public bool TryGetProfile(string name, out TTTProfile profile) => profileLookup.TryGetValue(name, out profile);

    //    public void NewProfile(string name)
    //    {
    //        if (ProfileExistsWithName(name)) return;

    //        TTTProfile profile = new TTTProfile(name);
    //        profileLookup.Add(name, profile);

    //        profileList.Add(profile);
    //        profileList.Sort((p1, p2) => StringComparer.OrdinalIgnoreCase.Compare(p1.Name, p2.Name));

    //        SaveProfiles();
    //    }

    //    public void DeleteProfile(string name)
    //    {
    //        if (!ProfileExistsWithName(name)) return;

    //        TTTProfile profile = profileLookup[name];
    //        profileLookup.Remove(name);
    //        profileList.Remove(profile);

    //        SaveProfiles();
    //    }

    //    public void SaveProfiles() => PlayerPrefs.SetString(ProfileListKey, JsonUtility.ToJson(this));

    //    public IEnumerator<TTTProfile> GetEnumerator()
    //    {
    //        foreach (TTTProfile profile in profileList)
    //        {
    //            yield return profile;
    //        }
    //    }

    //    #region Unity Callbacks

    //    void Awake()
    //    {
    //        PlayerPrefs.DeleteAll();

    //        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(ProfileListKey), this);
    //        foreach (TTTProfile profile in profileList)
    //        {
    //            if (profileLookup.ContainsKey(profile.Name))
    //            {
    //                Debug.LogError("Duplicate profile name: " + profile.Name);
    //                continue;
    //            }

    //            profileLookup.Add(profile.Name, profile);
    //        }
    //    }

    //    void OnApplicationQuit()
    //    {
    //        SaveProfiles();
    //    }

    //    #endregion

    //    #region Explicit Definitions

    //    int IReadOnlyCollection<TTTProfile>.Count => ProfileCount;

    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    //    #endregion
    //}
}
