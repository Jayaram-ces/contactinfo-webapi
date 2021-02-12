using AutoFixture;
using ContactInfoApi.Controllers;
using ContactInfoApi.Model;
using ContactInfoApi.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactInfoApi.Test
{
    public class ContactInfoControllerTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<ContactInfoController>> _loggerMock = new Mock<ILogger<ContactInfoController>>();
        private readonly Mock<IContactRepository> _contactRepositoryMock = new Mock<IContactRepository>();
        private readonly ContactInfoController _contactInfoController;

        public ContactInfoControllerTest()
        {
            _contactInfoController = new ContactInfoController(_loggerMock.Object, _contactRepositoryMock.Object);
        }


        #region Get all Contacts 

        [Fact]
        public async Task GetContacts_WhenContactServiceThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();

            _contactRepositoryMock.Setup(x => x.GetContactsAsync()).ThrowsAsync(exception);

            var actual = await _contactInfoController.GetContacts() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactRepositoryMock.Verify(m => m.GetContactsAsync(), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void GetContacts_WhenContactServiceSuccessful_ReturnsOkWithContacts()
        {
            var expected = _fixture.CreateMany<ContactInfoModel>();
            _contactRepositoryMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(expected.ToList);

            var actual = await _contactInfoController.GetContacts() as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.GetContactsAsync(), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Get Contact by Id
        [Fact]
        public async Task GetContact_WhenContactServiceThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();

            _contactRepositoryMock.Setup(x => x.GetContactAsync(request)).ThrowsAsync(exception);

            var actual = await _contactInfoController.GetContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactRepositoryMock.Verify(m => m.GetContactAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetContact_WhenContactServiceSuccessful_ReturnsOkWithContact()
        {
            var expected = _fixture.Create<ContactInfoModel>();
            _contactRepositoryMock.Setup(x => x.GetContactAsync(expected.Id)).ReturnsAsync(expected);

            var actual = await _contactInfoController.GetContact(expected.Id) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.GetContactAsync(expected.Id), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetContact_WhenContactServiceReturnNull_NotFoundResult()
        {
            string expected = "Not found in the directory.";
            ContactInfoModel reponse = null;
            var request = _fixture.Create<int>();
            _contactRepositoryMock.Setup(x => x.GetContactAsync(request)).ReturnsAsync(reponse);

            var actual = await _contactInfoController.GetContact(request) as NotFoundObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status404NotFound, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.GetContactAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Add Contact

        [Fact]
        public async Task AddContact_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            var request = _fixture.Build<ContactInfoModel>().Without(e => e.EmailId).Create();
            _contactInfoController.ModelState.AddModelError("EmailId", "Required");

            var actual = await _contactInfoController.AddContact(request) as BadRequestObjectResult;

            Assert.NotNull(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddContact_WhenContactServiceThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<ContactInfoModel>();
            _contactRepositoryMock.Setup(x => x.AddAsync(request)).ThrowsAsync(exception);

            var actual = await _contactInfoController.AddContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactRepositoryMock.Verify(m => m.AddAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void AddContact_WhenContactServiceSuccessful_ReturnsOk()
        {
            string expected = "Contact added successfully";
            var request = _fixture.Create<ContactInfoModel>();

            var actual = await _contactInfoController.AddContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.AddAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Update contact
        [Fact]
        public async Task UpdateContact_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            var request = _fixture.Build<ContactInfoModel>().Without(e => e.EmailId).Create();
            _contactInfoController.ModelState.AddModelError("EmailId", "Required");

            var actual = await _contactInfoController.UpdateContact(request) as BadRequestObjectResult;

            Assert.NotNull(actual);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateContact_WhenContactServiceThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<ContactInfoModel>();
            _contactRepositoryMock.Setup(x => x.UpdateAsync(request)).ThrowsAsync(exception);

            var actual = await _contactInfoController.UpdateContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactRepositoryMock.Verify(m => m.UpdateAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UpdateContact_WhenContactServiceSuccessful_ReturnsOk()
        {
            string expected = "Contact updated successfully";
            var request = _fixture.Create<ContactInfoModel>();

            var actual = await _contactInfoController.UpdateContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.UpdateAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion

        #region Delete contact
        [Fact]
        public async Task DeleteContact_WhenContactServiceThrowException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = _fixture.Create<AggregateException>();
            var request = _fixture.Create<int>();
            _contactRepositoryMock.Setup(x => x.DeleteAsync(request)).ThrowsAsync(exception);

            var actual = await _contactInfoController.DeleteContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status500InternalServerError, actual.StatusCode);
            Assert.Equal(exception.Message, actual.Value);
            _contactRepositoryMock.Verify(m => m.DeleteAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void DeleteContact_WhenContactServiceSuccessful_ReturnsOk()
        {
            string expected = "Contact Deleted successfully";
            var request = _fixture.Create<int>();

            var actual = await _contactInfoController.DeleteContact(request) as ObjectResult;

            Assert.NotNull(actual);
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal(expected, actual.Value);
            _contactRepositoryMock.Verify(m => m.DeleteAsync(request), Times.Once);
            _contactRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
