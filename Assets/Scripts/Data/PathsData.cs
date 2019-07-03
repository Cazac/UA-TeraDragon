using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all paths for each starting tiles to each ending tile
/// Paths can be retrived in 3 ways:
///  - paths: all paths from shortest to longest
///  - PathsByStarts: starting tiles as keys, retrieves all paths associated with key 
///  - PathsByEnd: ending tiles as keys, retrieves all paths associated with key 
/// </summary>
[Serializable]
public class PathsData {

    public List<List<WorldTile>> paths;
    public HashSet<List<WorldTile>> blockedPaths;
    public Dictionary<WorldTile, List<List<WorldTile>>> PathsByStart;
    public Dictionary<WorldTile, List<List<WorldTile>>> PathsByEnd;

    // just feed the list of paths and the dictionnaries are made from that
    public PathsData(List<List<WorldTile>> paths) {
        ListSize comparator = new ListSize();
        this.paths = paths;
        this.blockedPaths = new HashSet<List<WorldTile>>();

        PathsByStart = new Dictionary<WorldTile, List<List<WorldTile>>>();
        PathsByEnd = new Dictionary<WorldTile, List<List<WorldTile>>>();

        paths.Sort(comparator);


        foreach (List<WorldTile> wt in paths)
        {
            if (!PathsByStart.ContainsKey(wt[0]))
            {
                PathsByStart.Add(wt[0], new List<List<WorldTile>>() { wt });
            }
            else
            {
                PathsByStart[wt[0]].Add(wt);
            }

            if (!PathsByEnd.ContainsKey(wt[wt.Count - 1]))
            {
                PathsByEnd.Add(wt[wt.Count - 1], new List<List<WorldTile>>() { wt });
            }
            else
            {
                PathsByEnd[wt[wt.Count - 1]].Add(wt);
            }
        }
    }
}

// Comparor that sorts lists by size
class ListSize : IComparer<List<WorldTile>> {
    public int Compare(List<WorldTile> l1, List<WorldTile> l2) {
        if(l1.Count > l2.Count) {
            return 1;
        }
        else {
            return l1.Count.CompareTo(l2.Count);
        }
    }
}

