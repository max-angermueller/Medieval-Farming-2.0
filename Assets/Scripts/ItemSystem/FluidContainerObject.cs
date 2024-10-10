using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fluid Container Object", menuName = "Scriptable Object System/Items/FluidContainer")]

public class FluidContainerObject : ItemObject
{
    public FluidType fluidType = FluidType.None;
    public void Awake()
    {
        itemType = ItemType.FluidContainer;
    }
}
