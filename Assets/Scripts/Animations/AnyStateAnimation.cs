public enum RIG { BODY, LEGS }

public class AnyStateAnimation
{
    public RIG AnimationRig { get; set; }

    public string[] HigherPriority { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    public AnyStateAnimation(RIG rig, string name, params string[] higherPriority)
    {
        AnimationRig = rig;
        Name = name;
        HigherPriority = higherPriority;
    }


}
