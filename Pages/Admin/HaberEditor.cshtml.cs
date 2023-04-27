using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Admin
{
    public class HaberEditorModel : AdminModelBase<Haberler>
    {
        public HaberEditorModel(AesobDbContext context) : base(context)
        {
        }
    }
}
