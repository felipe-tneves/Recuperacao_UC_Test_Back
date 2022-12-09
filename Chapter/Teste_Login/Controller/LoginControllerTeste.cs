using Chapter.Controllers;
using Chapter.Interfaces;
using Chapter.Models;
using Chapter.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Teste_Login.Controller
{
    public class LoginControllerTeste
    {
        [Fact]
        public void LoginController_Retorno_Usuario_Invalido()
        {
            //Arrande - Prepara��o
            var repositoryEspelhado = new Mock<IUsuarioRepository>();

            repositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            var controller = new LoginController(repositoryEspelhado.Object);

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "test@teste.com";
            dadosUsuario.senha = "batata";

            //Act - A��o
            var resultado = controller.Login(dadosUsuario);

            //Assert - Verifica��o
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_Retorna_Token()
        {
            //Arrange - Prepara��o
            Usuario usuarioRetorno = new Usuario();
            usuarioRetorno.Email = "email@email.com";
            usuarioRetorno.Senha = "1234";
            usuarioRetorno.Tipo = "0";
            usuarioRetorno.Id = 1;

            var repositoryEspelhado = new Mock<IUsuarioRepository>();

            repositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetorno);

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "banana@email.com";
            dadosUsuario.senha = "banana";

            var controller = new LoginController(repositoryEspelhado.Object);

            string issuerValido = "chapter.webapi";

            //Act - A��o
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosUsuario);

            //token = 'sdfghjklxzcvbnm,wertyuiop'
            string tokenString = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenJwt = jwtHandler.ReadJwtToken(tokenString);

            //Assert - verifica��o
            Assert.Equal(issuerValido, tokenJwt.Issuer); 
        }



    }
}