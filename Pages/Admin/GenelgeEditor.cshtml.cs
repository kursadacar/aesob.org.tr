using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Admin
{
    public class GenelgeEditorModel : AdminModelBase<Genelgeler>
    {
        public GenelgeEditorModel(AesobDbContext context)
            : base(context)
        {
        }
    }
}
