using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Admin
{
    public class IcerikEditorModel : AdminModelBase<Icerikler>
    {
        public IcerikEditorModel(AesobDbContext context) : base(context)
        {
        }
    }
}
