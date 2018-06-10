using System.Security;

namespace HyberShift_CSharp.Model.Interface
{
    public interface IHavePassword
    {
        SecureString Password { get; }
        SecureString ConfirmPassword { get; }
    }
}