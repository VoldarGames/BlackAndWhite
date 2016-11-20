using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISpawnableBehaviour
    {
        GameObject GhostModel { get; set; }
        Material CanSpawnMaterial { get; set; }
        Material CantSpawnMaterial { get; set; }

    }
}
