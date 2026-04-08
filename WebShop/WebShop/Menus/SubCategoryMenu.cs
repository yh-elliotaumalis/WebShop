namespace HustlersAB.Admin.Menus;

public class SubCategoryMenu : MenuBase
{
    private readonly List<ProductSubCategory> _subCategories;
    private ProductSubCategory? _selected;

    public SubCategoryMenu(List<ProductSubCategory> subCategories)
    {
        _subCategories = subCategories;
        _options = subCategories
            .Select(sc => $"{sc.ParentCategory.Name} > {sc.Name}")
            .Append("Tillbaka")
            .ToArray();
    }

    public ProductSubCategory? SelectedSubCategory => _selected;

    protected override bool ExecuteChoice(int selectedIndex)
    {
        if (selectedIndex == _options.Length - 1)
            return true;

        _selected = _subCategories[selectedIndex];
        Console.Clear();
        return true;
    }

}
