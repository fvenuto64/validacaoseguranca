using System.Security;
using ValidarSenha.Core.Model;

namespace ValidarSenha.Core.Interfaces
{
    public interface IValidarSenha
    {
        SenhaResponse ExecutaValidacao(SecureString senha);

        bool ContarCaracteresRepetidos(SecureString senha);

    }
}
