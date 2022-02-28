namespace SomeCommerce.Web.Models
{
    public class IndexModel
    {
        public IndexModel(AgreementModel agreement)
        {
            Agreement = agreement;
        }

        public AgreementModel Agreement { get; set; }
    }
}
