using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Admin
{
    public class KurulEditorModel : AdminModelBase<KurulUyeleri>
    {
        public KurulEditorModel(AesobDbContext context)
            : base(context)
        {
        }
    }
}
