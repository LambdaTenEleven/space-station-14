using Content.Shared.Containers.ItemSlots;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Whitelist;

namespace Content.Shared.Nutrition.Components;

[RegisterComponent, Access(typeof(FoodPlateSystem))]
public sealed partial class FoodPlateComponent : Component
{
    public const string FoodSlotId = "food_slot";

    [DataField("foodSlot")]
    public ItemSlot FoodSlot = new()
    {
        Whitelist = new EntityWhitelist
        {
            Components = [nameof(FoodComponent)],
        }
    };
}
