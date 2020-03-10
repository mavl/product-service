using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Product.API.Controllers;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces;

namespace Unit.TestsProduct.API
{
    [TestClass]
    public class ProductControllerTests
    {
        #region GetAll test

        [TestMethod]
        public void GetAll()
        {
            var productController = new ProductController();
            var result = productController.GetAll(MockProductService().Object).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as OkObjectResult;
            Assert.IsNotNull(resultObject, "Returned result is not okObjectResult");
            var resultCollection = resultObject.Value as IEnumerable<Product.Domain.Product>;
            Assert.IsNotNull(resultCollection, "Returned result does not contain collection");
            Assert.AreEqual(3, resultCollection.Count(),"Returned collection contains bad count of products");
        }

        [TestMethod]
        public void GetAllInternalError()
        {
            var productController = new ProductController();
            var result = productController.GetAll(MockProductServiceWithError().Object).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as ObjectResult;
            Assert.IsNotNull(resultObject, "Returned result is not expectedResult");
            Assert.AreEqual(500,resultObject.StatusCode,"Status code is not matched");
        }

        #endregion

        #region GetById test

        [TestMethod]
        public void GetById()
        {
            const int requestedId = 1;
            var productController = new ProductController();
            var result = productController.GetById(MockProductService().Object, requestedId).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result.Result as OkObjectResult;
            Assert.IsNotNull(resultObject, "Returned result is not okObjectResult");
            var product = resultObject.Value as Product.Domain.Product;
            Assert.IsNotNull(product, "Returned product is null");
            Assert.AreEqual(requestedId, product.Id, "Returned product id is not matched");
        }

        [TestMethod]
        public void GetByIdNotFound()
        {
            const int requestedId = 4;
            var productController = new ProductController();
            var result = productController.GetById(MockProductService().Object, requestedId).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result.Result as NotFoundResult;
            Assert.IsNotNull(resultObject, "Returned result is not NotFoundObjectResult");
        }

        [TestMethod]
        public void GetByIdInternalError()
        {
            const int requestedId = 1;
            var productController = new ProductController();
            var result = productController.GetById(MockProductServiceWithError().Object, requestedId).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result.Result as ObjectResult;
            Assert.IsNotNull(resultObject, "Returned result is not expectedResult");
            Assert.AreEqual(500, resultObject.StatusCode, "Status code is not matched");
        }

        #endregion

        #region UpdateDescription test

        [TestMethod]
        public void UpdateDescription()
        {
            const int requestedId = 1;
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/Description", "test");

            var productController = new ProductController();
            var productService = MockProductServiceForUpdate(null);
            var result = productController.UpdateDescription(productService.Object, requestedId, jsonPatch).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as OkResult;
            Assert.IsNotNull(resultObject, "Returned result is not okObjectResult");
            productService.Verify(x => x.Update(requestedId, jsonPatch), Times.Once);
        }

        [TestMethod]
        public void UpdateDescriptionBadRequest()
        {
            const int requestedId = 1;
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/Id", "test");

            var productController = new ProductController();
            var productService = MockProductServiceForUpdate(null);
            var result = productController.UpdateDescription(productService.Object, requestedId, jsonPatch).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as BadRequestResult;
            Assert.IsNotNull(resultObject, "Returned result is not badRequestResult");
            productService.Verify(x => x.Update(requestedId, jsonPatch), Times.Never);
        }

        [TestMethod]
        public void UpdateDescriptionNotFound()
        {
            const int requestedId = 1;
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/Description", "test");

            var productController = new ProductController();
            var productService = MockProductServiceForUpdate(new NotFoundException());
            var result = productController.UpdateDescription(productService.Object, requestedId, jsonPatch).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as NotFoundResult;
            Assert.IsNotNull(resultObject, "Returned result is not NotFoundObjectResult");
            productService.Verify(x => x.Update(requestedId, jsonPatch), Times.Once);
        }

        [TestMethod]
        public void UpdateDescriptionInternalError()
        {
            const int requestedId = 1;
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/Description", "test");

            var productController = new ProductController();
            var productService = MockProductServiceForUpdate(new Exception());
            var result = productController.UpdateDescription(productService.Object, requestedId, jsonPatch).Result;

            Assert.IsNotNull(result, "Returned result is null");
            var resultObject = result as ObjectResult;
            Assert.IsNotNull(resultObject, "Returned result is not NotFoundObjectResult");
            Assert.AreEqual(500, resultObject.StatusCode, "Status code is not matched");
            productService.Verify(x => x.Update(requestedId, jsonPatch), Times.Once);
        }

        #endregion

        #region Private methods

        private Mock<IProductService> MockProductService()
        {
            var resultList = new List<Product.Domain.Product>()
            {
                new Product.Domain.Product() {Id = 1},
                new Product.Domain.Product() {Id = 2},
                new Product.Domain.Product() {Id = 3}
            };

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProducts())
                .Returns(Task.FromResult((IEnumerable<Product.Domain.Product>) resultList));
            productServiceMock.Setup(x => x.GetProduct(It.IsAny<long>()))
                .Returns((long id) =>
                {
                    var result = resultList.SingleOrDefault(product => product.Id == id);
                    if (result == null)
                        throw new NotFoundException();
                    return Task.FromResult(result);
                });

            return productServiceMock;
        }

        private Mock<IProductService> MockProductServiceWithError()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProducts()).Throws(new Exception());
            productServiceMock.Setup(x => x.GetProduct(It.IsAny<long>())).Throws(new Exception());

            return productServiceMock;
        }

        private Mock<IProductService> MockProductServiceForUpdate(Exception exception)
        {
            var productServiceMock = new Mock<IProductService>();
            
            if (exception != null)
                productServiceMock.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<JsonPatchDocument>()))
                    .Throws(exception);
            
            return productServiceMock;
        }

        #endregion
    }
}
