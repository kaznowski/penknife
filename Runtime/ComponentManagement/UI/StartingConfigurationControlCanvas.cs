using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleDash.CodingTools;
using DoubleDash.CodingTools.GenericDatabases;

namespace DoubleDash.ComponentManagement.UIManagement
{
    public class StartingConfigurationControlCanvas : MonoBehaviour, IVariable<ControlCanvases.StartingConfiguration>
    {
        [SerializeField]
        ControlCanvases.StartingConfiguration startingConfiguration;

        public ControlCanvases.StartingConfiguration Value { get => startingConfiguration; set => startingConfiguration = value; }
    }
}