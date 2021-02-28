using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TwoPlayersScript : NetworkBehaviour
{

    [SyncVar]
    public GameObject playerA;

    [SyncVar]
    public GameObject playerB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
