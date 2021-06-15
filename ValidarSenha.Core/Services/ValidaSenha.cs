using System;
using System.Linq;
using System.Collections.Generic;
using System.Security;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ValidarSenha.Core.Interfaces;
using ValidarSenha.Core.Model;
using System.ComponentModel;
using System.Text;

namespace ValidarSenha.Core.Services
{
    public class ValidaSenha : IValidarSenha
    {
        private readonly List<ExpressaoValidacao> listaExpressaoValidacao;

        private SenhaResponse senhaResponse;
        public ValidaSenha()
        {
            // Carrega a lista de regras expressoes regulares para validacao
            listaExpressaoValidacao = new List<ExpressaoValidacao>
            {

                new ExpressaoValidacao
                {
                    expressaoRegular = @"(?=^.{9,})",
                    mensagemErro = "Senha deve ter 9 ou mais caracteres"
                },

                new ExpressaoValidacao
                {
                    expressaoRegular = @"(?=.*[0-9].{1,}$)",
                    mensagemErro = "Senha deve ter ao menos 1 dígito"
                },

                new ExpressaoValidacao
                {
                    expressaoRegular = @"(?=.*[a-z].{1,}$)",
                    mensagemErro = "Senha deve ter ao menos 1 letra minúscula"
                },

                new ExpressaoValidacao
                {
                    expressaoRegular = @"(?=.*[A-Z].{1,}$)",
                    mensagemErro = "Senha deve ter ao menos 1 letra maiúscula"
                },

                new ExpressaoValidacao
                {
                    expressaoRegular = @"(?=.*[!@#$%^&*()-+].{1,}$)",
                    mensagemErro = "Senha deve ter ao menos 1 caractere especial (!@#$%^&*()-+)"
                }
            };


        }

        public SenhaResponse ExecutaValidacao(SecureString senha)
        {
            // variavel para pesquisa
            char espaco = '\u0020';

            // Comeca com sucesso na resposta
            senhaResponse = new SenhaResponse
            {
                mensagem = "Senha validada com sucesso",
                senhaValida = true
            };

            try
            {

                // Validacoes por expressoes regulares
                if (ValidacaoDePatterns(senha))
                {
                    // Patterns validos - verifica espacos em branco
                    if (convertToUnSecureString(senha).Contains(espaco))
                    {
                        senhaResponse.mensagem = "Senha não deve possuir espaços";
                        senhaResponse.senhaValida = false;
                    }
                    else
                    {
                        // Conta os caracteres repetidos se nao encontrou inconsistencia nas verificacoes anteriores
                        if (ContarCaracteresRepetidos(senha))
                        {
                            senhaResponse.mensagem = "Senha não deve possuir caracteres repetidos";
                            senhaResponse.senhaValida = false;
                        }
                    }

                }
           }
            catch(Exception x)
            {
                senhaResponse.mensagem = string.Format("Erro na validação de senha: {0}", x.Message.ToString());
                senhaResponse.senhaValida = false;
            }
            return senhaResponse;


        }

        private bool ValidacaoDePatterns(SecureString senha)
        {
            // Percorre a lista de validacoes e sai ao primeiro erro
            foreach (ExpressaoValidacao expressao in listaExpressaoValidacao)
            {
                // Monta o objeto com a expressao. Timeout para evitar DDOS
                Regex regex = new Regex(expressao.expressaoRegular, RegexOptions.None, new TimeSpan(0, 2, 0));

                // Valida a string de acordo com a regra
                if (!regex.Match(convertToUnSecureString(senha).Trim()).Success)
                {
                    // se nao deu match senha invalida
                    senhaResponse.mensagem = expressao.mensagemErro;
                    senhaResponse.senhaValida = false;
                    break;
                }

            }

            return senhaResponse.senhaValida;
        }

        public bool ContarCaracteresRepetidos(SecureString senha)
        {
            bool bRepetido = false;
            //Percorre as letras e conta
            foreach(Char ch in convertToUnSecureString(senha))
            {
                int freq = convertToUnSecureString(senha).Count(T => T == ch);
                if (freq > 1)
                {
                    bRepetido = true;
                    break;
                }
            }
            return bRepetido;
        }

        private string convertToUnSecureString(SecureString secstrSenha)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrSenha);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
