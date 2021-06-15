Projeto: Validar Senha
Tipo: WebAPI
Linguagem: C#
Plataforma: .NET Core 3.1

Descrição da solução:
    Uma REST API cuja informação de entrada é a senha a ser validada, e a saída é um Json contendo uma variável booleana indicando se a senha é válida ou não, e uma mensagem informando se a senha é válida ou não, e não sendo válida o motivo da inconsistência.

Racional da solução:
    Para validação de senha, considerei que a informação de entrada deve ser protegida contra possível vazamento. Desta forma, optei por utilizar o objeto SecureString, que impede a leitura da variável mesmo em dumps de memória. Sendo assim, logo no início do método a informação da API é colocada na SecureString e depois é limpa (string.empty)
    Para implementar as diversas validações, optei pela utilização de expressões regulares, e dividi a validação em etapas para poder devolver a mensagem de inconsistência, evitando que o chamador tenha que verificar o porquê da senha estar inválida. As validações de existência de espaços e caracteres repetidos foram separadas porque estas funções são muito simples no C# mas em expressões regulares ficaria muito mais complexo para resolver. 
    A decisão de deixar a resposta mais complexa do que um simples booleano foi pensada para liberar o cliente de ter que recriar as mesmas regras apenas para identificar qual foi a inconsistência.
    Com relação aos testes optei por utilizar o Moq, que é bastante utilizado aqui no banco e os times já possuem conhecimento da ferramenta, facilitando a manutenção.
    

Instruções básicas de como executar o projeto:
- Executar o projeto ValidarSenha.API no VSCode ou Visual Studio 2019
- Será aberto o browser na URL  https://localhost:5001
- Complementar a URL com o endereço da API https://localhost:5001/validarsenha/validar/<informe aqui a senha a ser validada>
- Se a senha contiver algum erro, a mensagem será exibida no browser conforme exemplo abaixo:
    {"mensagem":"Senha não deve possuir espaços","senhaValida":false}
- Se a senha for considerada válida, a mensagem abaixo será exibida:
    {"mensagem":"Senha validada com sucesso","senhaValida":true}
    
As regras de consistência executadas são:
1-Senha deve ter 9 ou mais caracteres
2-Senha deve ter ao menos 1 dígito
3-Senha deve ter ao menos 1 letra minúscula
4-Senha deve ter ao menos 1 letra maiúscula
5-Senha deve ter ao menos 1 caractere especial (!@#$%^&*()-+)
6-Senha não deve possuir espaços
7-Senha não deve possuir caracteres repetidos

Testes Unitários e Integrados

O projeto ValidarSenha.Tests possui 2 classes de testes:
1 - TestesUnitarios.cs onde estão todos os testes unitários executados diretamente sobre o serviço ValidarSenha do projeto ValidarSenha.Core
2 - TestesIntegrados.cs onde estão os testes que acessam a camada de controle do projeto ValidarSenha.API
