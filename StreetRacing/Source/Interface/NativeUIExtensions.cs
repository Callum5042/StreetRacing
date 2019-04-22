using NativeUI;

namespace StreetRacing.Source.Interface
{
    public static class NativeUIExtensions
    {
        public static void AddCheckbox(this UIMenu menu, bool value, string name)
        {
            var newitem = new UIMenuCheckboxItem(name, value);
            menu.AddItem(newitem);
            menu.OnCheckboxChange += (sender, item, @checked) =>
            {
                if (item == newitem)
                {
                    value = @checked;
                }
            };
        }
    }
}