using System.Collections.Generic;
using System.Linq;

namespace VetCore
{
    public class OwnerWithPets : Owner
    {
        public List<Pet> Pets { get; set; } = new List<Pet>();

        public string PetsInfo
        {
            get
            {
                if (Pets == null || !Pets.Any())
                    return "Нет питомцев ";

                return string.Join(", ", Pets.Select(p =>$"{p.Name} ({p.AnimalType}, {p.Breed}, {p.Age} лет)"));
            }
        }
        public int PetsCount => Pets?.Count ?? 0;
        public long PhoneAsNumber
        {
            get
            {
                if (string.IsNullOrEmpty(Phone))
                    return 0;

                string digits = new string(Phone.Where(char.IsDigit).ToArray());
                return long.TryParse(digits, out long num) ? num : 0;
            }
        }
    }
}