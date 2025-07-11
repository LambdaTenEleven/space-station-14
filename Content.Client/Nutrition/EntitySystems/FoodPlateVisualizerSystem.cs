using System.Numerics;
using Content.Shared.Nutrition;
using Content.Shared.Nutrition.Components;
using Robust.Client.GameObjects;

namespace Content.Client.Nutrition.EntitySystems;

public sealed class FoodPlateVisualizerSystem : VisualizerSystem<FoodPlateComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, FoodPlateComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!SpriteSystem.LayerMapTryGet((uid, args.Sprite), FoodPlateLayers.Food, out var layer, false))
            return;

        if (AppearanceSystem.TryGetData<string>(uid, FoodPlateVisuals.SpritePrototype, out var proto, args.Component)
            && !string.IsNullOrEmpty(proto))
        {
            var texture = SpriteSystem.GetPrototypeIcon(proto).Default;
            SpriteSystem.LayerSetTexture((uid, args.Sprite), layer, texture);
        }

        // If the sprite offset is set, apply it to the layer.
        if (AppearanceSystem.TryGetData<Vector2>(uid, FoodPlateVisuals.SpriteOffset, out var offset, args.Component))
            SpriteSystem.LayerSetOffset((uid, args.Sprite), layer, offset);

        var hasFood = AppearanceSystem.TryGetData<bool>(uid, FoodPlateVisuals.FoodPresent, out var present, args.Component) && present;
        SpriteSystem.LayerSetVisible((uid, args.Sprite), layer, hasFood);
    }
}

public enum FoodPlateLayers : byte
{
    Base,
    Food,
}
