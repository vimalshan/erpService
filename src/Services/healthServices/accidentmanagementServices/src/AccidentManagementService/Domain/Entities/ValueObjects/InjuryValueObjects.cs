using System;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Value Object for Injury Details
    /// </summary>
    public class InjuryDetails : IEquatable<InjuryDetails>
    {
        public long InjuryCategoryId { get; private set; }
        public long InjuryNatureId { get; private set; }
        public string BodyPart { get; private set; }
        public string Description { get; private set; }

        private InjuryDetails() { }

        public InjuryDetails(long injuryCategoryId, long injuryNatureId, string bodyPart, string description)
        {
            if (injuryCategoryId <= 0)
                throw new ArgumentException("Injury category ID must be greater than zero", nameof(injuryCategoryId));
            if (injuryNatureId <= 0)
                throw new ArgumentException("Injury nature ID must be greater than zero", nameof(injuryNatureId));
            if (string.IsNullOrWhiteSpace(bodyPart))
                throw new ArgumentException("Body part is required", nameof(bodyPart));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required", nameof(description));

            InjuryCategoryId = injuryCategoryId;
            InjuryNatureId = injuryNatureId;
            BodyPart = bodyPart;
            Description = description;
        }

        public override bool Equals(object? obj)
        {
            return obj is InjuryDetails details && Equals(details);
        }

        public bool Equals(InjuryDetails? other)
        {
            return other != null &&
                   InjuryCategoryId == other.InjuryCategoryId &&
                   InjuryNatureId == other.InjuryNatureId &&
                   BodyPart == other.BodyPart &&
                   Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InjuryCategoryId, InjuryNatureId, BodyPart, Description);
        }
    }

    /// <summary>
    /// Value Object for Accident Location and Circumstances
    /// </summary>
    public class AccidentCircumstances : IEquatable<AccidentCircumstances>
    {
        public string Location { get; private set; }
        public DateTime AccidentDateTime { get; private set; }
        public string Cause { get; private set; }
        public string PreventiveMeasures { get; private set; }

        private AccidentCircumstances() { }

        public AccidentCircumstances(string location, DateTime accidentDateTime, string cause, string preventiveMeasures)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required", nameof(location));
            if (accidentDateTime == default)
                throw new ArgumentException("Accident date/time is required", nameof(accidentDateTime));
            if (string.IsNullOrWhiteSpace(cause))
                throw new ArgumentException("Cause is required", nameof(cause));
            if (string.IsNullOrWhiteSpace(preventiveMeasures))
                throw new ArgumentException("Preventive measures are required", nameof(preventiveMeasures));

            Location = location;
            AccidentDateTime = accidentDateTime;
            Cause = cause;
            PreventiveMeasures = preventiveMeasures;
        }

        public override bool Equals(object? obj)
        {
            return obj is AccidentCircumstances circumstances && Equals(circumstances);
        }

        public bool Equals(AccidentCircumstances? other)
        {
            return other != null &&
                   Location == other.Location &&
                   AccidentDateTime == other.AccidentDateTime &&
                   Cause == other.Cause &&
                   PreventiveMeasures == other.PreventiveMeasures;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, AccidentDateTime, Cause, PreventiveMeasures);
        }
    }

    /// <summary>
    /// Value Object for Medical Treatment Information
    /// </summary>
    public class TreatmentInfo : IEquatable<TreatmentInfo>
    {
        public string MedicalCentreName { get; private set; }
        public DateTime MedicalCentreReceivedDate { get; private set; }
        public string TreatmentGiven { get; private set; }
        public string? Shift { get; private set; }
        public string? ShiftInchargeMan { get; private set; }

        private TreatmentInfo() { }

        public TreatmentInfo(string medicalCentreName, DateTime medicalCentreReceivedDate, string treatmentGiven, 
                            string? shift = null, string? shiftInchargeMan = null)
        {
            if (string.IsNullOrWhiteSpace(medicalCentreName))
                throw new ArgumentException("Medical centre name is required", nameof(medicalCentreName));
            if (medicalCentreReceivedDate == default)
                throw new ArgumentException("Medical centre received date is required", nameof(medicalCentreReceivedDate));
            if (string.IsNullOrWhiteSpace(treatmentGiven))
                throw new ArgumentException("Treatment given is required", nameof(treatmentGiven));

            MedicalCentreName = medicalCentreName;
            MedicalCentreReceivedDate = medicalCentreReceivedDate;
            TreatmentGiven = treatmentGiven;
            Shift = shift;
            ShiftInchargeMan = shiftInchargeMan;
        }

        public override bool Equals(object? obj)
        {
            return obj is TreatmentInfo info && Equals(info);
        }

        public bool Equals(TreatmentInfo? other)
        {
            return other != null &&
                   MedicalCentreName == other.MedicalCentreName &&
                   MedicalCentreReceivedDate == other.MedicalCentreReceivedDate &&
                   TreatmentGiven == other.TreatmentGiven &&
                   Shift == other.Shift &&
                   ShiftInchargeMan == other.ShiftInchargeMan;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MedicalCentreName, MedicalCentreReceivedDate, TreatmentGiven, Shift, ShiftInchargeMan);
        }
    }
}
