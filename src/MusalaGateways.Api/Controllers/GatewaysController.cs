using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusalaGateways.BusinessLogic.Interfaces;
using MusalaGateways.DataTransferObjects.Dtos;

namespace MusalaGateways.Api.Controllers
{
    /// <summary>
    /// Gateways API
    /// </summary>
    [Produces("application/json")]
    [Route("api/gateways")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GatewaysController : Controller
    {
        protected readonly IGatewayService _gatewayService;
        protected readonly IDeviceService _deviceService;

        /// <summary>
        /// Controller ctor
        /// </summary>
        /// <param name="gatewayService"></param>
        /// <param name="deviceService"></param>
        public GatewaysController(IGatewayService gatewayService, IDeviceService deviceService)
        {
            _gatewayService = gatewayService;
            _deviceService = deviceService;
        }

        #region READ

        /// <summary>
        /// Gets a gateway by id
        /// </summary>
        /// <param name="id">Gateway id</param>
        /// <returns>Gateway with the requested id</returns>
        /// <response code="200">Returns the gateway requested</response>
        /// <response code="404">The requested gateway doesn't exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GatewayDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GatewayDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var gateway = await _gatewayService.GetByIdAsync(id);
            if(gateway == null)
            {
                return NotFound();
            }
            return Ok(gateway);
        }

        /// <summary>
        /// Returns all gateways
        /// </summary>
        /// <param name="limit">Number of gateways to return, default all gateways</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GatewayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync(int limit = int.MaxValue)
        {
            var gateways = await _gatewayService.GetAllAsync(limit);
            return Ok(gateways);
        }

        /// <summary>
        /// Gets gateway's devices
        /// </summary>
        /// <param name="id">Gateway id</param>
        /// <returns>Gateway's devices</returns>
        /// <response code="200">Returns all gateway devices</response>
        /// <response code="404">The requested gateway doesn't exists</response>
        [HttpGet("{id}/devices")]
        [ProducesResponseType(typeof(IEnumerable<DeviceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGatewayDevicesByIdAsync([FromRoute] int id)
        {
            IEnumerable<DeviceDto> devices = await _gatewayService.GetGatewayDevicesAsync(id);
            if (!devices.Any())
                return NotFound();
            return Ok(devices);
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Creates a new gateway
        /// </summary>
        /// <param name="gatewayDto">Gateway data</param>
        /// <returns>The gateway info persisted</returns>
        /// <response code="200">The gateway was succesfully added</response>
        /// <response code="400">The Ipv4 address has not a valid value</response>
        [HttpPost]
        [ProducesResponseType(typeof(GatewayDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] GatewayDto gatewayDto)
        {
            IPAddress address;
            if (!IPAddress.TryParse(gatewayDto.Ipv4Address, out address))
            {
                return BadRequest("Invalid Ipv4 address");
            }
            var dto = await _gatewayService.CreateAsync(gatewayDto);
            return Ok(dto);
        }

        /// <summary>
        /// Adds a new device to an existing gateway
        /// </summary>
        /// <param name="id">Gateway id</param>
        /// <param name="device">Device data</param>
        /// <returns>Device data persisted</returns>
        /// <response code="200">The device was succesfully added to gateway</response>
        /// <response code="400">Gateway id parameter and new device data's gatewayId have different values</response>
        /// <response code="409">Max number of devices exceeded for this gateway</response>
        [HttpPost("{id}/devices")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddGatewayDeviceAsync([FromRoute] int id,
            [FromBody] DeviceDto device)
        {
            var gatewayDto = await _gatewayService.GetByIdAsync(id);

            if (id != device.GatewayId || gatewayDto == null)
            {
                return BadRequest();
            }

            if(gatewayDto.DevicesIds.Count() > 10)
            {
                return Conflict("Max number of devices exceeded for this gateway");
            }

            var deviceDto = await _deviceService.CreateAsync(device);

            return Ok(deviceDto);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Removes an existing gateway
        /// </summary>
        /// <param name="id">Gateway id</param>
        /// <returns>Gateway's deleted data</returns>
        /// <response code="200">Gateway removed</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(GatewayDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var dto = await _gatewayService.DeleteAsync(id);
            return Ok(dto);
        }

        /// <summary>
        /// Removes a device from an existing gateway
        /// </summary>
        /// <param name="id">Gateway id</param>
        /// <param name="deviceId">Device id</param>
        /// <returns>Data of removed device</returns>
        /// <response code="200">Device succesfully removed</response>
        /// <response code="400">Gateway doesn't exists or gateway doesn't contains the device</response>
        [HttpDelete("{id}/devices/{deviceId}")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemovesGatewayDeviceAsync([FromRoute] int id, [FromRoute] int deviceId)
        {
            var gatewayDto = await _gatewayService.GetByIdAsync(id);

            if (gatewayDto == null || !gatewayDto.DevicesIds.Contains(deviceId))
            {
                return BadRequest();
            }

            var deviceDto = await _deviceService.DeleteAsync(deviceId);

            return Ok(deviceDto);
        }

        #endregion
    }
}
