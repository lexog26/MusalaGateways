using AutoMapper;
using Moq;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.BusinessLogic.Services;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using MusalaGateways.DataTransferObjects.Dtos;
using MusalaGateways.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusalaGateways.BusinessLogicUnitTest
{
    public class DeviceServiceUnitTest
    {
        private IDeviceService _service;
        private Mock<IRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IUnitOfWork> _mockUow;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUow = new Mock<IUnitOfWork>();
            _service = new DeviceService(_mockRepo.Object, _mockMapper.Object, _mockUow.Object);
        }

        [TestCase(1,1, 3)]
        [TestCase(1, 1, 10)]
        public void CreateDeviceTest(int deviceId, int gatewayId, int gatewayDevices)
        {
            var gateway = new Gateway
            {
                Id = gatewayId,
                Devices = new Device[gatewayDevices]
            };
            _mockRepo.Setup(x => x.GetEntityByIdAsync<Gateway, int>(It.IsAny<int>(), It.IsAny<bool>()))
               .Returns(() => Task.FromResult(gateway));
            _mockRepo.Setup(x => x.Create<Device, int>(It.IsAny<Device>()))
                .Returns(() => deviceId);
            _mockMapper.Setup(x => x.Map<Device>(It.IsAny<DeviceDto>()))
                .Returns(() => new Device
                {
                    Id = deviceId,
                    GatewayId = gatewayId
                });
            try
            {
                var result = _service.CreateAsync(new DeviceDto { Id = deviceId, GatewayId = gatewayId }).Result;
                Assert.AreEqual(result.Id, deviceId);
                Assert.AreEqual(result.GatewayId, gatewayId);
            }
            catch (Exception e)
            {
                //Test case for gateway devices max number
                Assert.IsTrue(gateway.Devices.Count() >= 10);
            }
        }

        [TestCase(1)]
        public void DeleteDeviceTest(int deviceId)
        {
            _mockRepo.Setup(x => x.Delete<Device, int>(It.IsAny<int>()))
                .Returns(() => new Device { Id = deviceId });

            _mockMapper.Setup(x => x.Map<DeviceDto>(It.IsAny<Device>()))
                .Returns(() => new DeviceDto
                {
                    Id = deviceId
                });
            var result = _service.DeleteAsync(deviceId).Result;
            Assert.AreEqual(result.Id, deviceId);
        }
    }
}
