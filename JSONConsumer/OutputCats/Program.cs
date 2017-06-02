using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONConsumer;

namespace OutputCats
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = args[0];
            string petTypeFilter = args[1];

            var retrievedJson = JsonReader.GetJson(url).Result;

            var deserializedData = JsonReader.DeserializeJsonToPersonRecords(retrievedJson);

            var processedData = PetProcessor.ProcessPetOwners(deserializedData,petTypeFilter);

            OutputProcessPetOwners(processedData);

            Console.WriteLine("Press Any Key to exit");
            Console.ReadKey();
            
        }

        static void OutputProcessPetOwners(Dictionary<string, List<string>> processedData)
        {
            foreach (var group in processedData)
            {
                Console.WriteLine(group.Key);
                foreach (var pet in group.Value)
                {
                    Console.WriteLine('\t' + pet);
                }

                Console.WriteLine('\n');
            }
        }
    }
}
