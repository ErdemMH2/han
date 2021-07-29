using UnityEngine;
using System.Collections.Generic;

namespace HAN.Lib.Mvc.Model
{
    public class DenemeScripti : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ListModel<string> model = new ListModel<string>();
            TableId id = model.Append("IvırZıvır");
            UnityEngine.Debug.Log(model.Value(id));
            model.Assign(id, "ZıvırIvır");
            UnityEngine.Debug.Log(model.Value(id));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

