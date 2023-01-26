using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogBrandService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogBrandService>> _logger;

        private readonly CatalogBrand _testBrandForAdd = new ()
        {
            Brand = "testBrand"
        };

        private readonly CatalogBrand _testBrandForUpdateDeleteSuccess = new ()
        {
            Id = 1,
            Brand = "testBrand"
        };

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogBrandService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            int testResult = 1;

            _catalogBrandRepository.Setup(s => s.AddAsync(
                It.Is<string>(i => i == _testBrandForAdd.Brand))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testBrandForAdd.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.AddAsync(
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.AddAsync(_testBrandForAdd.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task GetCatalogBrandsAsync_Success()
        {
            // arrange
            var brandsSuccess = new Items<CatalogBrand>()
            {
                Data = new List<CatalogBrand>()
                {
                    new CatalogBrand()
                    {
                        Brand = "TestBrand"
                    }
                }
            };

            var catalogBrandSuccess = new CatalogBrand()
            {
                Brand = "TestBrand"
            };

            var catalogBrandDtoSuccess = new CatalogBrandDto()
            {
                Brand = "TestBrand"
            };

            _catalogBrandRepository.Setup(s => s.GetAsync()).ReturnsAsync(brandsSuccess);

            _mapper.Setup(s => s.Map<CatalogBrandDto>(
                It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

            // act
            var result = await _catalogBrandService.GetCatalogBrandsAsync();

            // arrange
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogBrandsAsync_Failed()
        {
            // arrange
            _catalogBrandRepository.Setup(s => s.GetAsync()).Returns((Func<Items<CatalogBrandDto>>)null!);

            // act
            var result = await _catalogBrandService.GetCatalogBrandsAsync();

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            int testId = 1;
            string testBrand = "TestBrandUpdated";
            bool testResult = true;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testBrandForUpdateDeleteSuccess);

            _testBrandForUpdateDeleteSuccess.Brand = testBrand;

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogBrand>(i => i == _testBrandForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.UpdateAsync(1, _testBrandForUpdateDeleteSuccess.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            int testId = 190;
            bool testResult = false;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Returns((Func<CatalogBrandDto>)null!);

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.Is<CatalogBrand>(i => i == _testBrandForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.UpdateAsync(testId, _testBrandForUpdateDeleteSuccess.Brand);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            int testId = 1;
            bool testResult = true;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_testBrandForUpdateDeleteSuccess);

            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogBrand>(i => i == _testBrandForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.DeleteAsync(testId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            int testId = 190;
            bool testResult = false;

            _catalogBrandRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Returns((Func<CatalogBrand>)null!);

            _catalogBrandRepository.Setup(s => s.DeleteAsync(
                It.Is<CatalogBrand>(i => i == _testBrandForUpdateDeleteSuccess))).ReturnsAsync(testResult);

            // act
            var result = await _catalogBrandService.DeleteAsync(1);

            // assert
            result.Should().Be(testResult);
        }
    }
}
