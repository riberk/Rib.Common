namespace Rib.Common.Helpers.Mailing
{
    using JetBrains.Annotations;

    public interface IMailBuilderFactory
    {
        [NotNull]
        IMailBuilder Create();
    }
}