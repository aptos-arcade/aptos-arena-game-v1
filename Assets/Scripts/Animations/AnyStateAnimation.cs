using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RIG { BODY, LEGS }

public class AnyStateAnimation
{
    public RIG AnimationRig { get; set; }

    public string[] HigherPriority { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    public AnyStateAnimation(RIG rig, string name, params string[] higherPriority)
    {
        this.AnimationRig = rig;
        this.Name = name;
        this.HigherPriority = higherPriority;
    }


}
