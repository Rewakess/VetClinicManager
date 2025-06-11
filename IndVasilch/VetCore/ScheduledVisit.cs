using System;

namespace VetCore
{
    public class ScheduledVisit
    {
        public int ID { get; set; }
        public string PetName { get; set; }
        public int Age { get; set; }
        public int OwnerID { get; set; }
        public DateTime VisitTime { get; set; }
        public string Reason { get; set; }
        public string Breed { get; set; }
        public string AnimalType { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }

        public static bool operator ==(ScheduledVisit visit1, ScheduledVisit visit2)
        {
            if (ReferenceEquals(visit1, null) && ReferenceEquals(visit2, null))
                return true;
            if (ReferenceEquals(visit1, null) || ReferenceEquals(visit2, null))
                return false;
            return visit1.ID == visit2.ID;
        }

        public static bool operator !=(ScheduledVisit visit1, ScheduledVisit visit2)
        {
            return !(visit1 == visit2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return this == (ScheduledVisit)obj;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"{PetName} - {VisitTime:g} - {Reason}";
        }
    }
}
