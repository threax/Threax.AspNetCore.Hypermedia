namespace Threax.AspNetCore.Halcyon.Ext.ValueProviders
{
    /// <summary>
    /// This interface will generate a title from a given name.
    /// </summary>
    public interface IAutoTitleGenerator
    {
        string CreateTitle(string name);
    }
}