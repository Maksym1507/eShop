﻿using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;

namespace Catalog.UnitTests.Services
{
    public class CatalogServiceTest
    {
        private readonly ICatalogService _catalogService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogItem _catalogItemSuccess = new ()
        {
            Title = "TestTitle"
        };

        private readonly CatalogItemDto _catalogItemDtoSuccess = new ()
        {
            Title = "TestTitle"
        };

        public CatalogServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
        }

        [Fact]
        public async void GetCatalogItemsAsync_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 4;
            var testTotalCount = 12;

            var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
            {
                Data = new List<CatalogItem>()
                {
                    new CatalogItem()
                    {
                        Title = "TestTitle",
                    }
                },
                TotalCount = testTotalCount
            };

            _catalogItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

            _mapper.Setup(s => s.Map<CatalogItemDto>(
                It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

            // act
            var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
        }

        [Fact]
        public async Task GetCatalogItems_Failed()
        {
            // arrange
            var testPageIndex = 1000;
            var testPageSize = 10000;

            _catalogItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItems<CatalogItemDto>>)null!);

            // act
            var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogItemByIdAsync_Success()
        {
            // arrange
            var testId = 1;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).ReturnsAsync(_catalogItemSuccess);

            _mapper.Setup(s => s.Map<CatalogItemDto>(
                It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

            // act
            var result = await _catalogService.GetCatalogItemByIdAsync(testId);

            // assert
            result.Should().NotBeNull();
            result.Should().Be(_catalogItemDtoSuccess);
        }

        [Fact]
        public async Task GetCatalogItemByIdAsync_Failed()
        {
            // arrange
            var testId = 190;

            _catalogItemRepository.Setup(s => s.GetByIdAsync(
                It.Is<int>(i => i == testId))).Returns((Func<CatalogItemDto>)null!);

            // act
            var result = await _catalogService.GetCatalogItemByIdAsync(testId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogItemsByBrandAsync_Success()
        {
            // arrange
            var testBrand = "TestBrand";

            var itemsSuccess = new Items<CatalogItem>()
            {
                Data = new List<CatalogItem>()
                {
                    new CatalogItem()
                    {
                        Title = "TestTitle"
                    }
                }
            };

            _catalogItemRepository.Setup(s => s.GetByBrandAsync(
                It.Is<string>(i => i == testBrand))).ReturnsAsync(itemsSuccess);

            _mapper.Setup(s => s.Map<CatalogItemDto>(
                It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

            // act
            var result = await _catalogService.GetCatalogItemsByBrandAsync(testBrand);

            // arrange
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogItemsByBrandAsync_Failed()
        {
            // arrange
            var testBrand = "TestBrand";
            _catalogItemRepository.Setup(s => s.GetByBrandAsync(
                It.Is<string>(i => i == testBrand))).Returns((Func<Items<CatalogItemDto>>)null!);

            // act
            var result = await _catalogService.GetCatalogItemsByBrandAsync(testBrand);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogItemsByTypeAsync_Success()
        {
            // arrange
            var testType = "TestType";

            var itemsSuccess = new Items<CatalogItem>()
            {
                Data = new List<CatalogItem>()
                {
                    new CatalogItem()
                    {
                        Title = "TestTitle"
                    }
                }
            };

            _catalogItemRepository.Setup(s => s.GetByTypeAsync(
                It.Is<string>(i => i == testType))).ReturnsAsync(itemsSuccess);

            _mapper.Setup(s => s.Map<CatalogItemDto>(
                It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

            // act
            var result = await _catalogService.GetCatalogItemsByTypeAsync(testType);

            // arrange
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCatalogItemsByTypeAsync_Failed()
        {
            // arrange
            var testType = "TestType";

            _catalogItemRepository.Setup(s => s.GetByTypeAsync(
                It.Is<string>(i => i == testType))).Returns((Func<Items<CatalogItemDto>>)null!);

            // act
            var result = await _catalogService.GetCatalogItemsByTypeAsync(testType);

            // assert
            result.Should().BeNull();
        }
    }
}