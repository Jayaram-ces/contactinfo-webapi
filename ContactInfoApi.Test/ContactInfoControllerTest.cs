using AutoFixture;
using ContactInfoApi.Controllers;
using ContactInfoApi.Model;
using ContactInfoApi.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactInfoApi.Test
{
    public class ContactInfoControllerTest
    {
        private static ContactReposistory repository;
        public static DbContextOptions<ContactInfoContext> dbContextOptions { get; }
        public static string connectionString = "ContactDB";

        static ContactInfoControllerTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ContactInfoContext>().UseInMemoryDatabase(connectionString).Options;

            var context = new ContactInfoContext(dbContextOptions);
            DummyContactInfoDBInitializer.Seed(context);
            repository = new ContactReposistory(context);
        }


        #region Get all Contacts 

        [Fact]
        public async void Task_GetContacts_Return_OkResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);

            //Act  
            var data = await controller.GetContacts();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetContacts_Return_BadRequestResult()
        {
            //Arrange  
            repository.db = null;
            var controller = new ContactInfoController(repository);

            //Act  
            var data = controller.GetContacts();

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
        }

        [Fact]
        public async void Task_GetContacts_MatchResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);

            //Act  
            var data = await controller.GetContacts();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var contact = okResult.Value.Should().BeAssignableTo<List<ContactInfo>>().Subject;

            Assert.Equal("Willam", contact[0].FirstName);
            Assert.Equal("Shakes", contact[0].LastName);
            Assert.Equal("+4142353246", contact[0].MobileNumber);
            Assert.Equal("Willam@outlook.com", contact[0].EmailId);


            Assert.Equal("Johnny", contact[1].FirstName);
            Assert.Equal("Deep", contact[1].LastName);
            Assert.Equal("+9842353246", contact[1].MobileNumber);
            Assert.Equal("Johnny@outlook.com", contact[1].EmailId);
        }
        #endregion

        #region Get Contact 

        [Fact]
        public async void Task_GetContactById_Return_OkResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 2;

            //Act  
            var data = await controller.GetContact(id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetContactById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 6;

            //Act  
            var data = await controller.GetContact(id);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetcontactById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            int? id = null;

            //Act  
            var data = await controller.GetContact(id);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetContactById_MatchResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            int? id = 1;

            //Act  
            var data = await controller.GetContact(id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var contact = okResult.Value.Should().BeAssignableTo<ContactInfo>().Subject;

            Assert.Equal("Willam", contact.FirstName);
            Assert.Equal("Shakes", contact.LastName);
            Assert.Equal("+4142353246", contact.MobileNumber);
            Assert.Equal("Willam@outlook.com", contact.EmailId);
        }

        #endregion

        #region Add Contact  

        [Fact]
        public async void Task_AddContact_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var contact = new ContactInfo() { Id = 7, FirstName = "Daniel", LastName = "Bajaji", MobileNumber = "+8142353246", EmailId = "Daniel@outlook.com" };

            //Act  
            var data = await controller.AddContact(contact);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_AddContact_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            ContactInfo contact = new ContactInfo() { Id = 6, FirstName = "Christopher", LastName = "Nolan", MobileNumber = "+8142353246"};

            //Act              
            var data = await controller.AddContact(contact);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
            var okResult = data.Should().BeOfType<BadRequestObjectResult>().Subject;
            Assert.Equal("The email address is required", okResult.Value);
        }

        [Fact]
        public async void Task_AddContact_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var contact = new ContactInfo() { Id = 6, FirstName = "Christopher", LastName = "Nolan", MobileNumber = "+8142353246", EmailId = "chris@outlook.com" };

            //Act  
            var data = await controller.AddContact(contact);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            Assert.Equal("Contact added successfully", okResult.Value);
        }

        #endregion

        #region Update Contact 

        [Fact]
        public async void Task_UpdateContact_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 2;

            //Act  
            var existingContact = await controller.GetContact(id);
            var okResult = existingContact.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<ContactInfo>().Subject;

            var UpdateContact = new ContactInfo();
            UpdateContact.Id = result.Id;
            UpdateContact.FirstName = "Johnny Updated";
            UpdateContact.LastName = result.LastName;
            UpdateContact.MobileNumber = result.MobileNumber;
            UpdateContact.EmailId = result.EmailId;


            var updatedData = await controller.UpdateContact(result.Id, UpdateContact);

            //Assert  
            Assert.IsType<OkObjectResult>(updatedData);
        }

        [Fact]
        public async void Task_UpdateContact_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 2;

            //Act  
            var existingContact = await controller.GetContact(id);
            var okResult = existingContact.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<ContactInfo>().Subject;


            var UpdateContact = new ContactInfo();
            UpdateContact.Id = result.Id;
            UpdateContact.FirstName = "Johnny Updated";
            UpdateContact.LastName = result.LastName;
            UpdateContact.MobileNumber = result.MobileNumber;
            UpdateContact.EmailId = result.EmailId;

            var data = await controller.UpdateContact(3, UpdateContact);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_UpdateContact_NullId_Return_BadRequest()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 2;

            //Act  
            var existingContact = await controller.GetContact(id);
            var okResult = existingContact.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<ContactInfo>().Subject;


            var UpdateContact = new ContactInfo();
            UpdateContact.Id = result.Id;
            UpdateContact.FirstName = "Johnny Updated";
            UpdateContact.LastName = result.LastName;
            UpdateContact.MobileNumber = result.MobileNumber;
            UpdateContact.EmailId = result.EmailId;

            var data = await controller.UpdateContact(null, UpdateContact);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion

        #region Delete Conatct  

        [Fact]
        public async void Task_DeleteContact_Return_OkResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 5;

            //Act  
            var data = await controller.DeleteContact(id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_DeleteContact_InvalidId_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            var id = 5;

            //Act  
            var data = await controller.DeleteContact(id);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_DeleteContact_NullId_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new ContactInfoController(repository);
            int? id = null;

            //Act  
            var data = await controller.DeleteContact(id);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion
    }
}
