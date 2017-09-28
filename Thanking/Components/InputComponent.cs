﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thanking.Attributes;
using Thanking.Variables;
using UnityEngine;

namespace Thanking.Components
{
    [Component]
    public class InputComponent : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(TriggerbotOptions.Toggle))
                TriggerbotOptions.Enabled = !TriggerbotOptions.Enabled;
        }
    }
}
