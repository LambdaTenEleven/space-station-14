using Content.Shared.Containers.ItemSlots;
using Content.Shared.Nutrition.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Nutrition.EntitySystems;

public sealed partial class FoodPlateSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FoodPlateComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<FoodPlateComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<FoodPlateComponent, EntInsertedIntoContainerMessage>(OnInserted);
        SubscribeLocalEvent<FoodPlateComponent, EntRemovedFromContainerMessage>(OnRemoved);
    }

    private void OnInit(EntityUid uid, FoodPlateComponent comp, ComponentInit args)
    {
        _slots.AddItemSlot(uid, FoodPlateComponent.FoodSlotId, comp.FoodSlot);
        UpdateAppearance(uid, comp);
    }

    private void OnRemove(EntityUid uid, FoodPlateComponent comp, ComponentRemove args)
    {
        _slots.RemoveItemSlot(uid, comp.FoodSlot);
    }

    private void OnInserted(EntityUid uid, FoodPlateComponent comp, EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != comp.FoodSlot.ID)
            return;
        UpdateAppearance(uid, comp);
    }

    private void OnRemoved(EntityUid uid, FoodPlateComponent comp, EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != comp.FoodSlot.ID)
            return;
        UpdateAppearance(uid, comp);
    }

    private void UpdateAppearance(EntityUid uid, FoodPlateComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        if (TryComp(uid, out AppearanceComponent? appearance))
        {
            _appearance.SetData(uid, FoodPlateVisuals.FoodPresent, comp.FoodSlot.HasItem, appearance);

            string prototype = string.Empty;
            if (comp.FoodSlot.Item is { Valid: true } item)
            {
                prototype = MetaData(item).EntityPrototype?.ID ?? string.Empty;
            }

            _appearance.SetData(uid, FoodPlateVisuals.SpritePrototype, prototype, appearance);
        }
    }
}
