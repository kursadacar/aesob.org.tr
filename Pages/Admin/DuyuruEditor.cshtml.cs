using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Admin
{
    public class DuyuruEditorModel : AdminModelBase<Duyurular>
    {
        public DuyuruEditorModel(AesobDbContext context)
            : base(context)
        {
        }
    }
}
