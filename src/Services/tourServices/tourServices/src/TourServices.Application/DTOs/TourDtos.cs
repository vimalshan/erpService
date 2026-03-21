namespace TourServices.Application.DTOs;

public record TourPackageDto(
    long TourId,
    string TourName,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal TourPackageCost,
    int MaxParticipants,
    string TourStatus,
    int ActiveRegistrations,
    long CreatedBy,
    DateTime CreatedOn,
    long? ModifiedBy,
    DateTime? ModifiedOn);

public record TourRegistrationDto(
    long RegistrationId,
    long TourId,
    string TourName,
    long ParticipantId,
    DateOnly RegistrationDate,
    string RegistrationStatus,
    long CreatedBy,
    DateTime CreatedOn);
