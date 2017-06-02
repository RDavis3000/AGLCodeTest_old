using System.Collections.Generic;
using System.Linq;


namespace JSONConsumer
{
    public static class PetProcessor
    {
        public static Dictionary<string,List<string>> ProcessPetOwners(IEnumerable<PetOwnerRecord> petOwners,string petTypeFilter)
        {

            var processedData = petOwners
                .SelectMany
                (
                    owners => owners.Pets.Where(pet => pet.Type.ToUpper() == petTypeFilter),
                    (owner, pet) => new {owner.Gender, pet.Name}
                )
                .OrderBy(pet => pet.Name)
                .GroupBy(owner => owner.Gender, pet => pet.Name)
                .ToDictionary(i => i.Key,j=>j.ToList());

            return processedData;            
        }
    }
}
