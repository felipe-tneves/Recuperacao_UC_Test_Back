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
            //Arrande - Preparação
            var repositoryEspelhado = new Mock<IUsuarioRepository>();

            repositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            var controller = new LoginController(repositoryEspelhado.Object);

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "test@teste.com";
            dadosUsuario.senha = "batata";

            //Act - Ação
            var resultado = controller.Login(dadosUsuario);

            //Assert - Verificação
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_Retorna_Token()
        {
            //Arrange - Preparação
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

            //Act - Ação
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosUsuario);

            //token = 'sdfghjklxzcvbnm,wertyuiop'
            string tokenString = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenJwt = jwtHandler.ReadJwtToken(tokenString);

            //Assert - verificação
            Assert.Equal(issuerValido, tokenJwt.Issuer); 
        }



    }
}