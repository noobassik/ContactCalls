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
			return StatusCode(500, new { message = "Произошла ошибка при получении контактов", error = ex.Message });
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ContactDto>> GetContact(int id)
	{
		try
		{
			var contact = await _contactService.GetContactByIdAsync(id);
			if (contact == null)
				return NotFound(new { message = "Контакт не найден" });

			return Ok(contact);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении контакта", error = ex.Message });
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
			return StatusCode(500, new { message = "Произошла ошибка при создании контакта", error = ex.Message });
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
				return BadRequest(new { message = "Несоответствие ID" });

			var contact = await _contactService.UpdateContactAsync(id, dto);
			if (contact == null)
				return NotFound(new { message = "Контакт не найден" });

			return Ok(contact);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при обновлении контакта", error = ex.Message });
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteContact(int id)
	{
		try
		{
			var result = await _contactService.DeleteContactAsync(id);
			if (!result)
				return NotFound(new { message = "Контакт не найден" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при удалении контакта", error = ex.Message });
		}
	}

	[HttpGet("{id}/profile")]
	public async Task<ActionResult<ContactProfileDto>> GetContactProfile(int id)
	{
		try
		{
			var profile = await _contactService.GetContactProfileAsync(id);
			if (profile == null)
				return NotFound(new { message = "Профиль контакта не найден" });

			return Ok(profile);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении профиля контакта", error = ex.Message });
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
			return StatusCode(500, new { message = "Произошла ошибка при создании профиля контакта", error = ex.Message });
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
				return NotFound(new { message = "Профиль контакта не найден" });

			return Ok(profile);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при обновлении профиля контакта", error = ex.Message });
		}
	}

	[HttpDelete("{id}/profile")]
	public async Task<ActionResult> DeleteContactProfile(int id)
	{
		try
		{
			var result = await _contactService.DeleteContactProfileAsync(id);
			if (!result)
				return NotFound(new { message = "Профиль контакта не найден" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при удалении профиля контакта", error = ex.Message });
		}
	}
}