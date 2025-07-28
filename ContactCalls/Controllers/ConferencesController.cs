using ContactCalls.Dtos;
using ContactCalls.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactCalls.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConferencesController : ControllerBase
{
	private readonly IConferenceService _conferenceService;

	public ConferencesController(IConferenceService conferenceService)
	{
		_conferenceService = conferenceService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ConferenceDto>>> GetConferences()
	{
		try
		{
			var conferences = await _conferenceService.GetAllConferencesAsync();
			return Ok(conferences);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ��������� �����������", error = ex.Message });
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ConferenceDto>> GetConference(int id)
	{
		try
		{
			var conference = await _conferenceService.GetConferenceByIdAsync(id);
			if (conference == null)
				return NotFound(new { message = "����������� �� �������" });

			return Ok(conference);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ��������� �����������", error = ex.Message });
		}
	}

	[HttpPost]
	public async Task<ActionResult<ConferenceDto>> CreateConference([FromBody] CreateConferenceRequest request)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var utcStartTime = request.StartTime.Kind == DateTimeKind.Unspecified
				? DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc)
				: request.StartTime.ToUniversalTime();

			var conference = await _conferenceService.CreateConferenceAsync(request.Name, utcStartTime);
			return CreatedAtAction(nameof(GetConference), new { id = conference.Id }, conference);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� �����������", error = ex.Message });
		}
	}

	[HttpPost("{id}/participants")]
	public async Task<ActionResult> AddParticipant(int id, [FromBody] AddParticipantRequest request)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _conferenceService.AddParticipantAsync(id, request.PhoneId);
			if (!result)
				return BadRequest(new { message = "�� ������� �������� ���������. ����������� ��� ������� �� �������, ���� �������� ��� ����������." });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ���������� ���������", error = ex.Message });
		}
	}

	[HttpDelete("{id}/participants/{phoneId}")]
	public async Task<ActionResult> RemoveParticipant(int id, int phoneId)
	{
		try
		{
			var result = await _conferenceService.RemoveParticipantAsync(id, phoneId);
			if (!result)
				return NotFound(new { message = "�������� �� ������" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� ���������", error = ex.Message });
		}
	}

	[HttpPost("{id}/start")]
	public async Task<ActionResult> StartConference(int id)
	{
		try
		{
			var result = await _conferenceService.StartConferenceAsync(id);
			if (!result)
				return BadRequest(new { message = "�� ������� ��������� �����������. ����������� �� ������� ��� �� � ��������������� �������." });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ������� �����������", error = ex.Message });
		}
	}

	[HttpPost("{id}/end")]
	public async Task<ActionResult> EndConference(int id)
	{
		try
		{
			var result = await _conferenceService.EndConferenceAsync(id);
			if (!result)
				return BadRequest(new { message = "�� ������� ��������� �����������. ����������� �� ������� ��� �� � ��������." });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ���������� �����������", error = ex.Message });
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteConference(int id)
	{
		try
		{
			var result = await _conferenceService.DeleteConferenceAsync(id);
			if (!result)
				return NotFound(new { message = "����������� �� �������" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� �����������", error = ex.Message });
		}
	}
}

public class CreateConferenceRequest
{
	public string Name { get; set; } = string.Empty;
	public DateTime StartTime { get; set; }
}

public class AddParticipantRequest
{
	public int PhoneId { get; set; }
}