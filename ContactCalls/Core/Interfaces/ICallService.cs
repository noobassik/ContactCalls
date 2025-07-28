using ContactCalls.Dtos;

namespace ContactCalls.Services.Interfaces;

public interface ICallService
{
	Task<IEnumerable<CallDto>> GetAllCallsAsync();
	Task<CallDto?> GetCallByIdAsync(int id);
	Task<IEnumerable<CallDto>> GetCallsByPhoneIdAsync(int phoneId);
	Task<IEnumerable<CallDto>> GetCallsByContactIdAsync(int contactId);
	Task<IEnumerable<CallDto>> GetCallsByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<CallDto> CreateCallAsync(CreateCallDto dto);
	Task<CallDto?> UpdateCallAsync(int id, UpdateCallDto dto);
	Task<bool> DeleteCallAsync(int id);
	Task<CallStatisticsDto> GetCallStatisticsAsync(int? phoneId, int? contactId, DateTime startDate, DateTime endDate);
	Task<BillingDto> GetBillingReportAsync(int? phoneId, int? contactId, DateTime startDate, DateTime endDate);
}