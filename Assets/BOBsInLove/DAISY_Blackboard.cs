
using UnityEngine;

public class DAISY_Blackboard : DynamicBlackboard
{
    private GameObject[] corners;

    private string[] utterances =
    {
        "Will he come or not?",
        "Sometimes I think he's an *d**t",
        "I love him, don't I?",
        "I doubt he has ever read a book",
        "Drinking beer in the bar, sure",
        "Doesn't f*ck*ng know what I need",
        "He'll come empty-handed", 
        "He's cute, don't you agree?",
        "Found him in Tinder. We matched",
        "We're made for each other",
        "He's funny when he's tipsy",
        "Have you heard him singing?",
        "He knows I love flowers",
        "I love his silly moustache",
        "Does he feel anything for me?",
        "A rich guy is what I need",
        "Hasn't worked a single day",
        "He's such a pr*ck!",
        "He's handsome, ain't he?"
    };

    public GameObject home;
    public GameObject hearts;

    void Start()
    {
        corners = GameObject.FindGameObjectsWithTag("CORNER");
        if (corners.Length == 0)
        {
            Debug.Log("No object CORNER found when starting Daisy's BB");
        }
    }

    public GameObject GetRandomCorner()
    {
        // get a random corner
        return corners[Random.Range(0, corners.Length)];
    }

    public string GetRandomUtterance ()
    {
        return utterances[Random.Range(0, utterances.Length)];
    }

    public void CloseDoor()
    {
        GameObject door = FindChildWithTag(home, "DOOR");
        if (door != null) door.SetActive(true);
    }

    private static GameObject FindChildWithTag(GameObject go, string tag)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == tag)
                return go.transform.GetChild(i).gameObject;
        }
        return null;
    }

    
}
