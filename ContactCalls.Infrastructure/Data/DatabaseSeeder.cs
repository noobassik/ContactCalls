using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactCalls.Infrastructure.Data;

public static class DatabaseSeeder
{
	public static async Task InitializeAsync(ApplicationDbContext context, ILogger logger)
	{
		try
		{
			logger.LogInformation("Инициализация базы данных...");

			await context.Database.EnsureCreatedAsync();

			if (await context.Contacts.AnyAsync())
			{
				logger.LogInformation("База данных уже содержит данные. Пропускаем заполнение");
				return;
			}

			logger.LogInformation("Начинаем заполнение базы данных начальными данными...");

			var contacts = await SeedContactsAsync(context, logger);
			var phones = await SeedPhonesAsync(context, contacts, logger);
			await SeedCallsAsync(context, phones, logger);
			await SeedConferencesAsync(context, phones, logger);

			logger.LogInformation("Заполнение базы данных завершено успешно");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при инициализации базы данных");
			throw;
		}
	}

	private static async Task<List<Contact>> SeedContactsAsync(ApplicationDbContext context, ILogger logger)
	{
		logger.LogInformation("Создание контактов...");

		var contacts = new List<Contact>
		{
			new Contact
			{
				FirstName = "Иван",
				LastName = "Петров",
				MiddleName = "Сергеевич",
				CreatedAt = DateTime.UtcNow.AddDays(-30)
			},
			new Contact
			{
				FirstName = "Мария",
				LastName = "Сидорова",
				MiddleName = "Александровна",
				CreatedAt = DateTime.UtcNow.AddDays(-25)
			},
			new Contact
			{
				FirstName = "Алексей",
				LastName = "Козлов",
				CreatedAt = DateTime.UtcNow.AddDays(-20)
			},
			new Contact
			{
				FirstName = "Елена",
				LastName = "Морозова",
				MiddleName = "Викторовна",
				CreatedAt = DateTime.UtcNow.AddDays(-15)
			},
			new Contact
			{
				FirstName = "Дмитрий",
				LastName = "Волков",
				MiddleName = "Михайлович",
				CreatedAt = DateTime.UtcNow.AddDays(-10)
			},
			new Contact
			{
				FirstName = "Анна",
				LastName = "Белова",
				CreatedAt = DateTime.UtcNow.AddDays(-5)
			}
		};

		try
		{
			await context.Contacts.AddRangeAsync(contacts);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} контактов", contacts.Count);

			await SeedContactProfilesAsync(context, contacts, logger);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании контактов");
			throw;
		}

		return contacts;
	}

	private static async Task SeedContactProfilesAsync(ApplicationDbContext context, List<Contact> contacts, ILogger logger)
	{
		logger.LogInformation("Создание профилей контактов...");

		var profiles = new List<ContactProfile>
		{
			new ContactProfile
			{
				ContactId = contacts[0].Id,
				Email = "ivan.petrov@techcompany.ru",
				DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 5, 15), DateTimeKind.Utc),
				Company = "ООО Технологии",
				Position = "Менеджер по продажам",
				Address = "г. Москва, ул. Ленина, д. 10, кв. 25",
				Notes = "Постоянный клиент с 2020 года. Предпочитает звонки в первой половине дня."
			},
			new ContactProfile
			{
				ContactId = contacts[1].Id,
				Email = "maria.sidorova@business.com",
				DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 8, 22), DateTimeKind.Utc),
				Company = "ИП Сидорова М.А.",
				Position = "Индивидуальный предприниматель",
				Address = "г. Санкт-Петербург, пр. Невский, д. 25, оф. 12",
				Notes = "Специализируется на маркетинговых услугах"
			},
			new ContactProfile
			{
				ContactId = contacts[2].Id,
				Email = "alex.kozlov@dev.ru",
				DateOfBirth = DateTime.SpecifyKind(new DateTime(1988, 12, 3), DateTimeKind.Utc),
				Company = "DevStudio Ltd",
				Position = "Senior Developer",
				Address = "г. Новосибирск, ул. Красный проспект, д. 100",
				Notes = "Опытный разработчик, работает удаленно"
			},
			new ContactProfile
			{
				ContactId = contacts[3].Id,
				Email = "elena.morozova@analytics.com",
				DateOfBirth = DateTime.SpecifyKind(new DateTime(1992, 3, 18), DateTimeKind.Utc),
				Company = "Корпорация Будущего",
				Position = "Бизнес-аналитик",
				Address = "г. Екатеринбург, ул. Мира, д. 15",
				Notes = "Эксперт по анализу данных и бизнес-процессов"
			},
			new ContactProfile
			{
				ContactId = contacts[4].Id,
				Email = "dmitry.volkov@consulting.ru",
				Company = "Консалтинговая группа",
				Position = "Старший консультант",
				Notes = "Специализируется на IT-консалтинге"
			}
		};

		try
		{
			await context.ContactProfiles.AddRangeAsync(profiles);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} профилей контактов", profiles.Count);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании профилей контактов");
			throw;
		}
	}

	private static async Task<List<Phone>> SeedPhonesAsync(ApplicationDbContext context, List<Contact> contacts, ILogger logger)
	{
		logger.LogInformation("Создание телефонных номеров...");

		var phones = new List<Phone>();

		var phoneData = new[]
		{
			("+7-495-123-45-67", "Рабочий"),
			("+7-926-987-65-43", "Мобильный"),
			("+7-812-555-12-34", "Офис"),
			("+7-921-444-55-66", "Мобильный"),
			("+7-383-999-11-22", "Основной"),
			("+7-343-888-77-66", "Рабочий"),
			("+7-912-333-44-55", "Личный"),
			("+7-495-777-88-99", "Офис"),
			("+7-925-666-77-88", "Мобильный"),
			("+7-812-222-33-44", "Дополнительный")
		};

		int phoneIndex = 0;
		for (int contactIndex = 0; contactIndex < contacts.Count; contactIndex++)
		{
			var contact = contacts[contactIndex];
			int phoneCount = contactIndex == 0 || contactIndex == 1 || contactIndex == 3 || contactIndex == 5 ? 2 : 1;

			for (int i = 0; i < phoneCount && phoneIndex < phoneData.Length; i++)
			{
				var (number, description) = phoneData[phoneIndex];

				phones.Add(new Phone
				{
					ContactId = contact.Id,
					Number = number,
					Description = description,
					IsPrimary = i == 0,
					CreatedAt = contact.CreatedAt.AddHours(i)
				});

				phoneIndex++;
			}
		}

		try
		{
			await context.Phones.AddRangeAsync(phones);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} телефонных номеров", phones.Count);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании телефонов");
			throw;
		}

		return phones;
	}

	private static async Task SeedCallsAsync(ApplicationDbContext context, List<Phone> phones, ILogger logger)
	{
		logger.LogInformation("Создание звонков...");

		if (phones.Count < 2)
		{
			logger.LogWarning("Недостаточно телефонов для создания звонков");
			return;
		}

		var calls = new List<Call>();
		var random = new Random(42);

		try
		{
			for (int i = 0; i < 30; i++)
			{
				var fromPhone = phones[random.Next(phones.Count)];
				var toPhone = phones[random.Next(phones.Count)];

				int attempts = 0;
				while ((toPhone.Id == fromPhone.Id || toPhone.ContactId == fromPhone.ContactId) && attempts < 10)
				{
					toPhone = phones[random.Next(phones.Count)];
					attempts++;
				}

				if (toPhone.Id == fromPhone.Id || toPhone.ContactId == fromPhone.ContactId)
				{
					continue;
				}

				var startTime = DateTime.UtcNow
					.AddDays(-random.Next(1, 31))
					.AddHours(-random.Next(8, 20))
					.AddMinutes(-random.Next(0, 60))
					.AddSeconds(-random.Next(0, 60));

				var statusValue = random.Next(1, 101);
				var status = statusValue switch
				{
					<= 70 => CallStatus.Completed,
					<= 85 => CallStatus.Missed,
					<= 95 => CallStatus.Declined,
					_ => CallStatus.Failed
				};

				var durationSeconds = status == CallStatus.Completed
					? random.Next(30, 1800)
					: 0;

				var endTime = status == CallStatus.Completed
					? startTime.AddSeconds(durationSeconds)
					: (DateTime?)null;

				var cost = status == CallStatus.Completed && durationSeconds > 0
					? Math.Round((decimal)(durationSeconds * (0.01 + random.NextDouble() * 0.04)), 2)
					: (decimal?)null;

				calls.Add(new Call
				{
					FromPhoneId = fromPhone.Id,
					ToPhoneId = toPhone.Id,
					StartTime = startTime,
					EndTime = endTime,
					DurationSeconds = durationSeconds,
					Status = status,
					Cost = cost
				});
			}

			await context.Calls.AddRangeAsync(calls);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} звонков", calls.Count);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании звонков");
			throw;
		}
	}

	private static async Task SeedConferencesAsync(ApplicationDbContext context, List<Phone> phones, ILogger logger)
	{
		logger.LogInformation("Создание конференций...");

		if (phones.Count < 3)
		{
			logger.LogWarning("Недостаточно телефонов для создания конференций");
			return;
		}

		var conferences = new List<Conference>
		{
			new Conference
			{
				Name = "Еженедельное совещание команды",
				StartTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-7).Date.AddHours(10), DateTimeKind.Utc),
				EndTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-7).Date.AddHours(11).AddMinutes(15), DateTimeKind.Utc),
				DurationSeconds = 4500,
				Status = ConferenceStatus.Completed
			},
			new Conference
			{
				Name = "Обсуждение проекта Alpha",
				StartTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-14).Date.AddHours(14), DateTimeKind.Utc),
				EndTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-14).Date.AddHours(15).AddMinutes(30), DateTimeKind.Utc),
				DurationSeconds = 5400,
				Status = ConferenceStatus.Completed
			},
			new Conference
			{
				Name = "Презентация нового продукта",
				StartTime = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(3).Date.AddHours(15), DateTimeKind.Utc),
				Status = ConferenceStatus.Scheduled
			}
		};

		try
		{
			await context.Conferences.AddRangeAsync(conferences);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} конференций", conferences.Count);

			await SeedConferenceParticipantsAsync(context, conferences, phones, logger);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании конференций");
			throw;
		}
	}

	private static async Task SeedConferenceParticipantsAsync(ApplicationDbContext context,
		List<Conference> conferences, List<Phone> phones, ILogger logger)
	{
		logger.LogInformation("Создание участников конференций...");

		var participants = new List<ConferenceParticipant>();
		var random = new Random(42);

		try
		{
			foreach (var conference in conferences.Where(c => c.Status == ConferenceStatus.Completed))
			{
				var participantCount = Math.Min(random.Next(3, 5), phones.Count);
				var selectedPhones = phones.OrderBy(x => random.Next()).Take(participantCount).ToList();

				for (int i = 0; i < selectedPhones.Count; i++)
				{
					var joinTime = conference.StartTime.AddMinutes(i * random.Next(0, 5));
					var leaveTime = conference.EndTime!.Value.AddMinutes(-random.Next(0, 10));

					participants.Add(new ConferenceParticipant
					{
						ConferenceId = conference.Id,
						PhoneId = selectedPhones[i].Id,
						JoinTime = joinTime,
						LeaveTime = leaveTime,
						DurationSeconds = (int)(leaveTime - joinTime).TotalSeconds
					});
				}
			}

			await context.ConferenceParticipants.AddRangeAsync(participants);
			await context.SaveChangesAsync();
			logger.LogInformation("Создано {Count} участников конференций", participants.Count);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при создании участников конференций");
			throw;
		}
	}
}