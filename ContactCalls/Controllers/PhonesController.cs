using ContactCalls.Dtos;
using ContactCalls.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactCalls.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhonesController : ControllerBase
{
	private readonly IPhoneService _phoneService;

	public PhonesController(IPhoneService phoneService)
	{
		_phoneService = phoneService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<PhoneDto>>> GetPhones()
	{
		try
		{
			var phones = await _phoneService.GetAllPhonesAsync();
			return Ok(phones);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении телефонов", error = ex.Message });
		}
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<PhoneDto>> GetPhone(int id)
	{
		try
		{
			var phone = await _phoneService.GetPhoneByIdAsync(id);
			if (phone == null)
				return NotFound(new { message = "Телефон не найден" });

			return Ok(phone);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении телефона", error = ex.Message });
		}
	}

	[HttpGet("by-number/{number}")]
	public async Task<ActionResult<PhoneDto>> GetPhoneByNumber(string number)
	{
		try
		{
			var phone = await _phoneService.GetPhoneByNumberAsync(number);
			if (phone == null)
				return NotFound(new { message = "Телефон не найден" });

			return Ok(phone);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении телефона", error = ex.Message });
		}
	}

	[HttpGet("by-contact/{contactId}")]
	public async Task<ActionResult<IEnumerable<PhoneDto>>> GetPhonesByContact(int contactId)
	{
		try
		{
			var phones = await _phoneService.GetPhonesByContactIdAsync(contactId);
			return Ok(phones);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при получении телефонов", error = ex.Message });
		}
	}

	[HttpPost]
	public async Task<ActionResult<PhoneDto>> CreatePhone([FromBody] CreatePhoneDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var phone = await _phoneService.CreatePhoneAsync(dto);
			return CreatedAtAction(nameof(GetPhone), new { id = phone.Id }, phone);
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
			return StatusCode(500, new { message = "Произошла ошибка при создании телефона", error = ex.Message });
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<PhoneDto>> UpdatePhone(int id, [FromBody] UpdatePhoneDto dto)
	{
		try
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (id != dto.Id)
				return BadRequest(new { message = "Несоответствие ID" });

			var phone = await _phoneService.UpdatePhoneAsync(id, dto);
			if (phone == null)
				return NotFound(new { message = "Телефон не найден" });

			return Ok(phone);
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при обновлении телефона", error = ex.Message });
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeletePhone(int id)
	{
		try
		{
			var result = await _phoneService.DeletePhoneAsync(id);
			if (!result)
				return NotFound(new { message = "Телефон не найден" });

			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при удалении телефона", error = ex.Message });
		}
	}

	[HttpPost("{id}/set-primary")]
	public async Task<ActionResult> SetPrimaryPhone(int id)
	{
		try
		{
			var result = await _phoneService.SetPrimaryPhoneAsync(id);
			if (!result)
				return NotFound(new { message = "Телефон не найден" });

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Произошла ошибка при установке основного телефона", error = ex.Message });
		}
	}
}