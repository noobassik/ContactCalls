using ContactCalls.Core.Mappings;
using ContactCalls.Infrastructure;
using ContactCalls.Infrastructure.Data;
using ContactCalls.Infrastructure.Settings;
using ContactCalls.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(DbSettings.Section));

builder.Services.AddInfrastructure();
builder.Services.AddServices();

builder.Services.AddAutoMapper(typeof(ContactMappingProfile));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();


	await DatabaseSeeder.InitializeAsync(context, logger);

	var contactsCount = await context.Contacts.CountAsync();
	var phonesCount = await context.Phones.CountAsync();
	var callsCount = await context.Calls.CountAsync();
	var conferencesCount = await context.Conferences.CountAsync();

	logger.LogInformation("Итоговая статистика: Контакты: {Contacts}, Телефоны: {Phones}, Звонки: {Calls}, Конференции: {Conferences}",
		contactsCount, phonesCount, callsCount, conferencesCount);

}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
else
{	
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();