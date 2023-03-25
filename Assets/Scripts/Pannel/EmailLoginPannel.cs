using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailLoginPannel : MonoBehaviour
{
    private void Update()
    {
        if (FirebaseAuthManager.Instance.userID.Length == 0)
            return;
        UIManager.Instance.NamePannelActive();
        gameObject.SetActive(false);
    }
}
