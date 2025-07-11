using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition;

[Serializable, NetSerializable]
public enum FoodPlateVisuals : byte
{
    FoodPresent,
    SpritePrototype,
    SpriteOffset,
}
