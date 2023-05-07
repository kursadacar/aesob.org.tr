namespace aesob.org.tr.Pages.Content.Base
{
    public interface INavigablePage
    {
        int MaxPages { get; }

        int CurrentPageIndex { get; }

        int MaxPerPage { get; }

        string ContentPageName { get; }

        string ContentParameterName { get; }
    }
}
