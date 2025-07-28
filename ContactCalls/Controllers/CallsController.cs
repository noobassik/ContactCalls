using ContactCalls.Dtos;
using ContactCalls.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactCalls.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CallsController : ControllerBase
{
	private readonly ICallService _callService;

	public CallsController(ICallService callService)
	{
		_callService = callService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CallDto>>> GetCalls(
		[FromQuery] int? phoneId = null,
		[FromQuery] int? contactId = null,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null)
	{
		try
		{
			IEnumerable<CallDto> calls;

			if (startDate.HasValue && endDate.HasValue)
			{
				calls = await _callService.GetCallsByDateRangeAsync(startDate.Value, endDate.Value);
			}
			else if (phoneId.HasValue)
			{
				calls = await _callService.GetCallsByPhoneIdAsync(phoneId.Value);
			}
			else if (contactId.HasValue)
			{
				calls = await _callService.GetCallsByContactIdAsync(contactId.Value);
			}
			else
			{
				calls = await _callService.GetAllCallsAsync();
			}

			return Ok(calls);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении звонков", error = ex.Message });
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<CallDto>> GetCall(int id)
	{
		try
		{
			var call = await _callService.GetCallByIdAsync(id);
			if (call == null)
				return NotFound(new { message = "Звонок не найден" });

			return Ok(call);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении звонка", error = ex.Message });
		}
	}

	[HttpPost]
	public async Task<ActionResult<CallDto>> CreateCall([FromBody] CreateCallDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var utcDto = new CreateCallDto
			{
				FromPhoneId = dto.FromPhoneId,
				ToPhoneId = dto.ToPhoneId,
				StartTime = dto.StartTime.Kind == DateTimeKind.Unspecified
					? DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc)
					: dto.StartTime.ToUniversalTime(),
				EndTime = dto.EndTime.HasValue
					? (dto.EndTime.Value.Kind == DateTimeKind.Unspecified
						? DateTime.SpecifyKind(dto.EndTime.Value, DateTimeKind.Utc)
						: dto.EndTime.Value.ToUniversalTime())
					: null,
				DurationSeconds = dto.DurationSeconds,
				Status = dto.Status,
				Cost = dto.Cost
			};

			var call = await _callService.CreateCallAsync(utcDto);
			return CreatedAtAction(nameof(GetCall), new { id = call.Id }, call);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при создании звонка", error = ex.Message });
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<CallDto>> UpdateCall(int id, [FromBody] UpdateCallDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (id != dto.Id)
				return BadRequest(new { message = "Несоответствие ID" });

			var utcDto = new UpdateCallDto
			{
				Id = dto.Id,
				FromPhoneId = dto.FromPhoneId,
				ToPhoneId = dto.ToPhoneId,
				StartTime = dto.StartTime.Kind == DateTimeKind.Unspecified
					? DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc)
					: dto.StartTime.ToUniversalTime(),
				EndTime = dto.EndTime.HasValue
					? (dto.EndTime.Value.Kind == DateTimeKind.Unspecified
						? DateTime.SpecifyKind(dto.EndTime.Value, DateTimeKind.Utc)
						: dto.EndTime.Value.ToUniversalTime())
					: null,
				DurationSeconds = dto.DurationSeconds,
				Status = dto.Status,
				Cost = dto.Cost
			};

			var call = await _callService.UpdateCallAsync(id, utcDto);
			if (call == null)
				return NotFound(new { message = "Звонок не найден" });

			return Ok(call);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при обновлении звонка", error = ex.Message });
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteCall(int id)
	{
		try
		{
			var result = await _callService.DeleteCallAsync(id);
			if (!result)
				return NotFound(new { message = "Звонок не найден" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при удалении звонка", error = ex.Message });
		}
	}

	[HttpGet("statistics")]
	public async Task<ActionResult<CallStatisticsDto>> GetCallStatistics(
		[FromQuery] int? phoneId = null,
		[FromQuery] int? contactId = null,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null)
	{
		try
		{
			var start = startDate ?? DateTime.Today.AddDays(-30);
			var end = endDate ?? DateTime.Today.AddDays(1);

			var statistics = await _callService.GetCallStatisticsAsync(phoneId, contactId, start, end);
			return Ok(statistics);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении статистики звонков", error = ex.Message });
		}
	}

	[HttpGet("billing")]
	public async Task<ActionResult<BillingDto>> GetBillingReport(
		[FromQuery] int? phoneId = null,
		[FromQuery] int? contactId = null,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null)
	{
		try
		{
			var start = startDate ?? DateTime.Today.AddDays(-30);
			var end = endDate ?? DateTime.Today.AddDays(1);

			var billing = await _callService.GetBillingReportAsync(phoneId, contactId, start, end);
			return Ok(billing);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении отчета биллинга", error = ex.Message });
		}
	}
}