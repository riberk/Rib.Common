namespace Rib.Common.Models.Metadata
{
    public interface IEnumModel
    {
        object Value { get; }

        string Name { get; }

        string Description { get; }
    }

    public interface IEnumModel<out TEnum>
        where TEnum : struct 
    {
        TEnum EnumValue { get; }

    }
}