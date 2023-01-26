﻿using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly ICatalogTypeService _catalogTypeService;

        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogTypeService>> _logger;

        private readonly CatalogType _testTypeForAdd = new ()
        {
            Type = "testType"
        };

        private readonly CatalogType _testTypeForUpdateDeleteSuccess = new ()
        {
            Id = 1,
            Type = "testType"
        };

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogTypeService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            int testResult = 1;

            _catalogTypeRepository.Setup(s => s.AddAsync(
                It.Is<string>(i => i == _testTypeForAdd.Type))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testTypeForAdd.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.AddAsync(
                It.Is<string>(i => i == _testTypeForAdd.Type))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.AddAsync(_testTypeForAdd.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task GetCatalogTypesAsync_Success()
        {
            var typesSuccess = new Items<CatalogType>()
            {
                Data = new List<CatalogType>()
                {
                    new CatalogType()
                    {
                        Type = "TestType"
                    }
                }
            };

            var catalogTypeSuccess = new CatalogType()
            {
                Type = "TestType"
            };

            var catalogTypeDtoSuccess = new CatalogTypeDto()
            {
                Type = "TestType"
            };

            // arrange
            _catalogTypeRepository.Setup(s => s.GetAsync()).ReturnsAsync(typesSuccess);

            _mapper.Setup(s => s.Map<CatalogTypeDto>(
                It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

            // act
            var result = await _catalogTypeService.GetCatalogTypesAsync();

            // arrange
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogTypesAsync_Failed()
        {
            // arrange
            _catalogTypeRepository.Setup(s => s.GetAsync()).Returns((Func<Items<CatalogBrandDto>>)null!);

            // act
            var result = await _catalogTypeService.GetCatalogTypesAsync();

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            int testId = 1;
            string testType = "TestTypeUpdated";
            bool testResult = true;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testTypeForUpdateDeleteSuccess);

            _testTypeForUpdateDeleteSuccess.Type = testType;

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogType>(i => i == _testTypeForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.UpdateAsync(1, _testTypeForUpdateDeleteSuccess.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            int testId = 190;
            bool testResult = false;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Returns((Func<CatalogBrandDto>)null!);

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogType>(i => i == _testTypeForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.UpdateAsync(testId, _testTypeForUpdateDeleteSuccess.Type);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            int testId = 1;
            bool testResult = true;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testTypeForUpdateDeleteSuccess);

            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogType>(i => i == _testTypeForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            int testId = 190;
            bool testResult = false;

            _catalogTypeRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Returns((Func<CatalogType>)null!);

            _catalogTypeRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogType>(i => i == _testTypeForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogTypeService.DeleteAsync(1);

            // assert
            result.Should().Be(testResult);
        }
    }
}
