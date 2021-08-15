using DevopsWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly ILogger<HomeController> _logger;

        [Fact]
        public void Test_GetAllBooks()
        {
            var controller = new HomeController(_logger);

            //Act  
            var data = controller.Book();

            //Assert  
            Assert.NotNull(data);
        }
    }
}
