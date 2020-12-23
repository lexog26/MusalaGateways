using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusalaGateways.BusinessLogic.Configurations.Mapper;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.BusinessLogic.Services;
using MusalaGateways.DataLayer.Context;
using MusalaGateways.DataLayer.Repository;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogicUnitTest
{
    public class GatewayServiceTest
    {
        private IGatewayService _service;
        private Mock<IRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IUnitOfWork> _mockUow;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUow = new Mock<IUnitOfWork>();
            _service = new GatewayService(_mockRepo.Object, _mockMapper.Object, _mockUow.Object);
        }

        [TestCase(1)]
        public void GetByIdTest(int id)
        {
            _mockRepo.Setup(x => x.GetEntityByIdAsync<Gateway, int>(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(new Gateway
                {
                    Id = id,
                    Name = "Gateway 1",
                    Ipv4Address = "192.145.68.24",
                    SerialNumber = "5632"
                }));
            _mockMapper.Setup(x => x.Map<GatewayDto>(It.IsAny<Gateway>()))
                .Returns(() => new GatewayDto
                {
                    Id = id
                });
            var result = _service.GetByIdAsync(id).Result;
            Assert.AreEqual(result.Id, id);
        }

        [TestCase(1, "192.145.68.24")]
        [TestCase(1, null)]
        public void CreateGatewayTest(int id, string ipv4Address)
        {
            _mockRepo.Setup(x => x.Create<Gateway, int>(It.IsAny<Gateway>()))
                .Returns(() => id);
            _mockMapper.Setup(x => x.Map<Gateway>(It.IsAny<GatewayDto>()))
                .Returns(() => new Gateway
                {
                    Id = id,
                    Ipv4Address = ipv4Address
                });
            try
            {
                var result = _service.CreateAsync(new GatewayDto { Id = id, Ipv4Address = ipv4Address }).Result;
                Assert.AreEqual(result.Id, id);
            }
            catch(InvalidOperationException e)
            {
                IPAddress address;
                //Test case for invalid Ipv4Address
                Assert.IsTrue(!IPAddress.TryParse(ipv4Address, out address));
            }
        }

        [Test]
        public void GetAllGatewaysTest()
        {
            var ids = new List<int>() { 1, 2 };

            _mockRepo.Setup(x => x.GetAllAsync<Gateway>(null, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(() => Task.FromResult(
                    new List<Gateway>()
                    {
                        new Gateway
                        {
                            Id = 1,
                            Name = "Gateway 1",
                            Ipv4Address = "192.145.68.24",
                            SerialNumber = "5632"
                        },
                        new Gateway
                        {
                            Id = 2,
                            Name = "Gateway 2",
                            Ipv4Address = "192.145.68.24",
                            SerialNumber = "5632"
                        }
                    }.AsEnumerable()
                 ));
            _mockMapper.Setup(x => x.Map<IEnumerable<GatewayDto>>(It.IsAny<IEnumerable<Gateway>>()))
                .Returns(() => new List<GatewayDto>()
                    {
                        new GatewayDto
                        {
                            Id = 1,
                        },
                        new GatewayDto
                        {
                            Id = 2,
                        }
                    });
            var resultIds = _service.GetAllAsync().Result.Select(x => x.Id);
            Assert.AreEqual(resultIds.Count(), ids.Count);
            Assert.IsTrue(resultIds.Where(x => ids.Contains(x)).Count() == ids.Count);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetGatewayDevicesTest(int id)
        {
            var gateway = new Gateway
            {
                Id = 1,
                Name = "Gateway 1",
                Ipv4Address = "192.145.68.24",
                SerialNumber = "5632",
                Devices = new List<Device>
                    {
                        new Device
                        {
                            Id = 1,
                            GatewayId = 1
                        }
                    }
            };
            _mockRepo.Setup(x => x.GetEntityByIdAsync<Gateway, int>(1, It.IsAny<bool>()))
                .Returns(() => Task.FromResult(gateway));

            _mockMapper.Setup(x => x.Map<IEnumerable<DeviceDto>>(It.IsAny<IEnumerable<Device>>()))
                .Returns(() => new List<DeviceDto>
                {
                    new DeviceDto
                    {
                       Id = 1,
                       GatewayId = 1
                    }
                });
            try
            {
                var result = _service.GetGatewayDevicesAsync(id).Result;
                Assert.AreEqual(result.Count(), gateway.Devices.Count());
                Assert.AreEqual(result.First().GatewayId, gateway.Id);
            }
            catch(Exception e)
            {
                //Gateway doesn't exists
                Assert.IsTrue(gateway.Id != id);
            }
            
        }
    }
}