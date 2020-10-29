namespace BioMad_backend.Areas.Api.V1.Models
{
    public class SignUpWithSocialAccountModel : SignUpModel
    {
        public SocialAuthenticationCore.Models.SocialAuthenticationIdentity Identity { get; set; }
    }
}