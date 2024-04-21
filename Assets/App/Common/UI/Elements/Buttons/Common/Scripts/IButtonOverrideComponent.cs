
namespace App.Common.UI.Elements.Buttons
{
    public interface IButtonOverrideComponent
    {
        string GetDefaultText();
        T GetAdditionalData<T>() where T : class;
    }
}