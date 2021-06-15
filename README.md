Projeto: Validar Senha
Tipo: WebAPI
Linguagem: C#
Plataforma: .NET Core 3.1

Descri��o da solu��o:
    Uma REST API cuja informa��o de entrada � a senha a ser validada, e a sa�da � um Json contendo uma vari�vel booleana indicando se a senha � v�lida ou n�o, e uma mensagem informando se a senha � v�lida ou n�o, e n�o sendo v�lida o motivo da inconsist�ncia.

Racional da solu��o:
    Para valida��o de senha, considerei que a informa��o de entrada deve ser protegida contra poss�vel vazamento. Desta forma, optei por utilizar o objeto SecureString, que impede a leitura da vari�vel mesmo em dumps de mem�ria. Sendo assim, logo no in�cio do m�todo a informa��o da API � colocada na SecureString e depois � limpa (string.empty)
    Para implementar as diversas valida��es, optei pela utiliza��o de express�es regulares, e dividi a valida��o em etapas para poder devolver a mensagem de inconsist�ncia, evitando que o chamador tenha que verificar o porqu� da senha estar inv�lida. As valida��es de exist�ncia de espa�os e caracteres repetidos foram separadas porque estas fun��es s�o muito simples no C# mas em express�es regulares ficaria muito mais complexo para resolver. 
    A decis�o de deixar a resposta mais complexa do que um simples booleano foi pensada para liberar o cliente de ter que recriar as mesmas regras apenas para identificar qual foi a inconsist�ncia.
    Com rela��o aos testes optei por utilizar o Moq, que � bastante utilizado aqui no banco e os times j� possuem conhecimento da ferramenta, facilitando a manuten��o.
    

Instru��es b�sicas de como executar o projeto:
- Executar o projeto ValidarSenha.API no VSCode ou Visual Studio 2019
- Ser� aberto o browser na URL  https://localhost:5001
- Complementar a URL com o endere�o da API https://localhost:5001/validarsenha/validar/<informe aqui a senha a ser validada>
- Se a senha contiver algum erro, a mensagem ser� exibida no browser conforme exemplo abaixo:
    {"mensagem":"Senha n�o deve possuir espa�os","senhaValida":false}
- Se a senha for considerada v�lida, a mensagem abaixo ser� exibida:
    {"mensagem":"Senha validada com sucesso","senhaValida":true}
    
As regras de consist�ncia executadas s�o:
1-Senha deve ter 9 ou mais caracteres
2-Senha deve ter ao menos 1 d�gito
3-Senha deve ter ao menos 1 letra min�scula
4-Senha deve ter ao menos 1 letra mai�scula
5-Senha deve ter ao menos 1 caractere especial (!@#$%^&*()-+)
6-Senha n�o deve possuir espa�os
7-Senha n�o deve possuir caracteres repetidos

Testes Unit�rios e Integrados

O projeto ValidarSenha.Tests possui 2 classes de testes:
1 - TestesUnitarios.cs onde est�o todos os testes unit�rios executados diretamente sobre o servi�o ValidarSenha do projeto ValidarSenha.Core
2 - TestesIntegrados.cs onde est�o os testes que acessam a camada de controle do projeto ValidarSenha.API
