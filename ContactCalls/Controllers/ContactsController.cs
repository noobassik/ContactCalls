using ContactCalls.Dtos;
using ContactCalls.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactCalls.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
	private readonly IContactService _contactService;

	public ContactsController(IContactService contactService)
	{
		_contactService = contactService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts([FromQuery] string? search = null)
	{
		try
		{
			var contacts = string.IsNullOrEmpty(search)
				? await _contactService.GetAllContactsAsync()
				: await _contactService.SearchContactsAsync(search);

			return Ok(contacts);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ��������� ���������", error = ex.Message });
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ContactDto>> GetContact(int id)
	{
		try
		{
			var contact = await _contactService.GetContactByIdAsync(id);
			if (contact == null)
				return NotFound(new { message = "������� �� ������" });

			return Ok(contact);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ��������� ��������", error = ex.Message });
		}
	}

	[HttpPost]
	public async Task<ActionResult<ContactDto>> CreateContact([FromBody] CreateContactDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var contact = await _contactService.CreateContactAsync(dto);
			return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� ��������", error = ex.Message });
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<ContactDto>> UpdateContact(int id, [FromBody] UpdateContactDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (id != dto.Id)
				return BadRequest(new { message = "�������������� ID" });

			var contact = await _contactService.UpdateContactAsync(id, dto);
			if (contact == null)
				return NotFound(new { message = "������� �� ������" });

			return Ok(contact);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ���������� ��������", error = ex.Message });
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteContact(int id)
	{
		try
		{
			var result = await _contactService.DeleteContactAsync(id);
			if (!result)
				return NotFound(new { message = "������� �� ������" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� ��������", error = ex.Message });
		}
	}

	[HttpGet("{id}/profile")]
	public async Task<ActionResult<ContactProfileDto>> GetContactProfile(int id)
	{
		try
		{
			var profile = await _contactService.GetContactProfileAsync(id);
			if (profile == null)
				return NotFound(new { message = "������� �������� �� ������" });

			return Ok(profile);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ��������� ������� ��������", error = ex.Message });
		}
	}

	[HttpPost("{id}/profile")]
	public async Task<ActionResult<ContactProfileDto>> CreateContactProfile(int id, [FromBody] CreateContactProfileDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var profile = await _contactService.CreateContactProfileAsync(id, dto);
			return CreatedAtAction(nameof(GetContactProfile), new { id }, profile);
		}
		catch (ArgumentException ex)
		{
			return NotFound(new { message = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� ������� ��������", error = ex.Message });
		}
	}

	[HttpPut("{id}/profile")]
	public async Task<ActionResult<ContactProfileDto>> UpdateContactProfile(int id, [FromBody] CreateContactProfileDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var profile = await _contactService.UpdateContactProfileAsync(id, dto);
			if (profile == null)
				return NotFound(new { message = "������� �������� �� ������" });

			return Ok(profile);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� ���������� ������� ��������", error = ex.Message });
		}
	}

	[HttpDelete("{id}/profile")]
	public async Task<ActionResult> DeleteContactProfile(int id)
	{
		try
		{
			var result = await _contactService.DeleteContactProfileAsync(id);
			if (!result)
				return NotFound(new { message = "������� �������� �� ������" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "��������� ������ ��� �������� ������� ��������", error = ex.Message });
		}
	}
}